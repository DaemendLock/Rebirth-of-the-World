using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;

using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public sealed class ActorActAllUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IAbilityFactory _abilityFactory;
        private readonly ActionFactory _actionFactory;

        private readonly ISkillExecutionPort _skillExecutionPort;
        private readonly ISkillActionStateChangeHandler _skillActionStateChangeHandler;
        private readonly ISkillHitHandler _skillHitHandler;

        private readonly IActorActionUpdateOutput _actorUpdateOutput;

        public ActorActAllUseCase(IUnitStore units, IPositionableRepository positionableRepository,
            IAbilityFactory abilityFactory, ActionFactory actionFactory,
            ISkillExecutionPort skillExecutionPort, ISkillActionStateChangeHandler skillActionStateChangeHandler,
            ISkillHitHandler skillHitHandler, IActorActionUpdateOutput actionUpdateOutput)
        {
            _unitStore = units;
            _positionableRepository = positionableRepository;
            _abilityFactory = abilityFactory;
            _actionFactory = actionFactory;
            _skillExecutionPort = skillExecutionPort;
            _skillActionStateChangeHandler = skillActionStateChangeHandler;
            _skillHitHandler = skillHitHandler;
            _actorUpdateOutput = actionUpdateOutput;
        }

        public void Execute(System.ReadOnlySpan<Updatable> targets, float deltaTime)
        {
            foreach (Updatable target in targets)
            {
                try
                {
                    ExecuteFor(target.Id, deltaTime * target.TimeScale);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        private void ExecuteFor(UnitId target, float deltaTime)
        {
            UnitContext unit = _unitStore.Get(target);

            UpdateAction(unit, deltaTime);

            Cast(unit);
            Move(unit);
        }

        private void UpdateAction(UnitContext unit, float deltaTime)
        {
            Action action = unit.Actor.CurrentAction;

            if (action == null)
            {
                return;
            }

            if (unit.LifeState.State != ConsciousState.Alive)
            {
                InterruptReason reason = unit.LifeState.State == ConsciousState.Dead ? InterruptReason.Death : InterruptReason.Downed;

                if (unit.Actor.TryInterrupt(reason))
                {
                    _actorUpdateOutput.Clear(unit.Id);

                    if (action.TryGet(out IAbilityAction abilityAction))
                    {
                        AbilityKey key = new(unit.Id, abilityAction.Source);
                        _skillHitHandler.Reset(key);
                        _skillActionStateChangeHandler.Handle(key, ActionState.Inactive);
                        return;
                    }
                }

                return;
            }

            if (action.IsComplete)
            {
                StopAction(unit, action);
                return;
            }

            bool isAbilityAction = action.TryGet(out IAbilityAction abilityActionStrategy);
            ActionState oldState = isAbilityAction ? abilityActionStrategy.State : default;

            action.Progress(deltaTime);

            if (action.Flags.HasFlag(ActionFlags.Holdable) && abilityActionStrategy.State == ActionState.Recovery)
            {
                _actorUpdateOutput.Release(unit.Id);
            }

            if (isAbilityAction && abilityActionStrategy.State != oldState)
            {
                HandleStateChanges(unit.Id, abilityActionStrategy, oldState);
                return;
            }
        }

        private void Move(UnitContext unit)
        {
            UnityEngine.Vector2 relativeDirection = new(unit.Actor.DesiredActions.MovementDirection.X, unit.Actor.DesiredActions.MovementDirection.Y);
            Positionable positionable = _positionableRepository.Get(unit.Id);

            if (unit.Actor.CanMove == false || unit.LifeState.State == ConsciousState.Dead)
            {
                UnityEngine.Vector3 velocity = positionable.Velocity;
                positionable.Velocity = new UnityEngine.Vector3(0, velocity.y, 0);
                _positionableRepository.Update(positionable);
                return;
            }

            if (relativeDirection.sqrMagnitude > 1)
            {
                relativeDirection = relativeDirection.normalized;
            }

            float speed = positionable.Speed;

            UnityEngine.Vector3 oldSpeed = positionable.Velocity;
            positionable.Velocity = (positionable.Rotation * new UnityEngine.Vector3(relativeDirection.x, 0, relativeDirection.y) * speed) + new UnityEngine.Vector3(0, oldSpeed.y, 0);
            _positionableRepository.Update(positionable);
        }

        private bool Cast(UnitContext unit)
        {
            if (unit.Actor.DesiredActions.Skill.HasValue == false)
            {
                return true;
            }

            SkillId skillId = unit.Actor.DesiredActions.Skill.Value;
            UnitId caster = unit.Id;
            unit.Actor.DesireCast(default);

            if (unit.LifeState.State != ConsciousState.Alive)
            {
                return true;
            }

            if (unit.Skills.GetCooldown(skillId) > 0)
            {
                return true;
            }

            AbilityKey abilityKey = new(caster, skillId);
            SkillInfo ability = _abilityFactory.Create(skillId);

            if (CanCast(unit, ability.Flags, abilityKey) != CastFailReason.Success)
            {
                UnityEngine.Debug.Log("Can't cast - cast forbidden");
                return true;
            }

            bool beginAction = _skillExecutionPort.BeginCast(abilityKey);

            if (ability.BaseCooldown > 0)
            {
                unit.Skills.SetCooldown(skillId, ability.BaseCooldown);
            }

            if (beginAction == false || unit.LifeState.State != ConsciousState.Alive)
            {
                return true;
            }

            if (ability.Actions != null && ability.Actions.Count > 0)
            {
                return StartCastAction(unit, ability.Actions.First(), skillId, ability.Flags);
            }

            return true;
        }

        private CastFailReason CanCast(UnitContext unit, SkillFlags skillFlags, AbilityKey abilityKey)
        {
            if (unit.LifeState.State != ConsciousState.Alive)
            {
                return CastFailReason.CantCast;
            }

            Action action = unit.Actor.CurrentAction;

            if (skillFlags.HasFlag(Common.Flags.SkillFlags.Instant) || action == null)
            {
                return _skillExecutionPort.CanCast(abilityKey);
            }

            if (action.TryGet(out IAbilityAction abilityAction) == false)
            {
                return CastFailReason.CastInProgress;
            }

            if (abilityAction.State != ActionState.Recovery)
            {
                if (abilityAction.State != ActionState.Inactive)
                {
                    return CastFailReason.CastInProgress;
                }
            }

            if (action.TryGet(out IChainableAction chainableAction) == false)
            {
                return CastFailReason.CastInProgress;
            }

            if (chainableAction.CanChainInto(abilityKey.Skill) == false)
            {
                return CastFailReason.CastInProgress;
            }

            return _skillExecutionPort.CanCast(abilityKey);
        }

        private bool StartCastAction(UnitContext unit, ActionId actionId, SkillId skillId, SkillFlags skillFlags)
        {
            if (unit.Actor.TryInterrupt(InterruptReason.Chained, out Action oldAction))
            {
                _actorUpdateOutput.Clear(unit.Id);

                if (oldAction.TryGet(out IAbilityAction oldAbilityAction))
                {
                    AbilityKey oldKey = new(unit.Id, oldAbilityAction.Source);
                    _skillHitHandler.Reset(oldKey);
                    _skillActionStateChangeHandler.Handle(oldKey, ActionState.Inactive);

                    if (unit.LifeState.State != ConsciousState.Alive)
                    {
                        return true;
                    }
                }
            }

            Action newAction = _actionFactory.CreateCastAction(actionId, skillId, skillFlags);
            unit.Actor.StartAction(newAction);
            _actorUpdateOutput.Present(unit.Id, newAction.Id, newAction.Flags, newAction.ActiveTime);
            _skillActionStateChangeHandler.Handle(new(unit.Id, skillId), ActionState.Startup);
            return true;
        }

        private void StopAction(UnitContext unit, Action action)
        {
            unit.Actor.StopAction();
            _actorUpdateOutput.Clear(unit.Id);

            if (action.TryGet(out IAbilityAction abilityAction))
            {
                AbilityKey key = new(unit.Id, abilityAction.Source);
                _skillHitHandler.Reset(key);
                _skillActionStateChangeHandler.Handle(key, ActionState.Inactive);
            }
        }

        private void HandleStateChanges(UnitId actorId, IAbilityAction action, ActionState oldState)
        {
            if (action.State == oldState)
            {
                return;
            }

            AbilityKey key = new(actorId, action.Source);
            _skillHitHandler.Reset(key);
            _skillActionStateChangeHandler.Handle(key, action.State);
        }
    }
}
