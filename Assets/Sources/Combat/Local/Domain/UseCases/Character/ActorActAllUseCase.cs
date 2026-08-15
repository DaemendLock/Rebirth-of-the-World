using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System.Linq;

namespace Combat.Local.Domain.UseCases.Character
{
    public sealed class ActorActAllUseCase
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IAbilityFactory _abilityFactory;
        private readonly ActionFactory _actionFactory;

        private readonly ISkillExecutionPort _skillExecutionPort;
        private readonly ISkillActionStateChangeHandler _skillActionStateChangeHandler;
        private readonly ISkillHitHandler _skillHitHandler;

        public ActorActAllUseCase(IAttributesRepository attributesRepository, IPositionableRepository positionableRepository,
            IActorRepository actorRepository, ISkillOwnerRepository skillOwnerRepository,
            IAbilityFactory abilityFactory, ActionFactory actionFactory, ISkillExecutionPort skillExecutionPort,
            ISkillActionStateChangeHandler skillActionStateChangeHandler, ISkillHitHandler skillHitHandler)
        {
            _attributesRepository = attributesRepository;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;
            _skillOwnerRepository = skillOwnerRepository;
            _abilityFactory = abilityFactory;
            _actionFactory = actionFactory;
            _skillExecutionPort = skillExecutionPort;
            _skillActionStateChangeHandler = skillActionStateChangeHandler;
            _skillHitHandler = skillHitHandler;
        }

        public void Execute(System.ReadOnlySpan<Updatable> targets, float deltaTime)
        {
            foreach (Updatable target in targets)
            {
                if (_actorRepository.TryGet(target.Id, out Actor actor) == false)
                {
                    continue;
                }

                ExecuteFor(actor, deltaTime * target.TimeScale);
            }
        }

        private void ExecuteFor(Actor actor, float deltaTime)
        {
            AttributesOwner attributesOwner = _attributesRepository.Get(actor.Id);

            UpdateAction(actor, deltaTime);

            if (_actorRepository.TryGet(actor.Id, out actor) == false)
            {
                return;
            }

            Cast(actor);
            Move(actor, attributesOwner);
        }

        private void UpdateAction(Actor actor, float deltaTime)
        {
            Action action = actor.CurrentAction;

            if (action == null)
            {
                return;
            }

            if (actor.ConsciousState != ConsciousState.Alive)
            {
                StopAction(actor);

                if (action.TryGet(out IAbilityAction abilityAction))
                {
                    _skillActionStateChangeHandler.Handle(new(actor.Id, abilityAction.Source), ActionState.Inactive);
                }

                return;
            }

            if (action.IsComplete)
            {
                StopAction(actor);
                return;
            }

            bool isAbilityAction = action.TryGet(out IAbilityAction abilityActionStrategy);
            ActionState oldState = isAbilityAction ? abilityActionStrategy.State : default;

            action.Progress(deltaTime);
            _actorRepository.Update(actor);

            if (isAbilityAction)
            {
                HandleStateChanges(actor.Id, abilityActionStrategy, oldState);
            }
        }

        private void Move(Actor actor, AttributesOwner attributesOwner)
        {
            UnityEngine.Vector2 relativeDirection = new(actor.DesiredActions.MovementDirection.X, actor.DesiredActions.MovementDirection.Y);

            if (actor.CanMove == false)
            {
                return;
            }

            if (relativeDirection.sqrMagnitude > 1)
            {
                relativeDirection = relativeDirection.normalized;
            }

            float speed = attributesOwner.GetAttributeValue(UnitAttribute.Speed);

            Positionable positionable = _positionableRepository.Get(actor.Id);

            UnityEngine.Vector3 oldSpeed = positionable.Velocity;
            positionable.Velocity = (positionable.Rotation * new UnityEngine.Vector3(relativeDirection.x, 0, relativeDirection.y) * speed) + new UnityEngine.Vector3(0, oldSpeed.y, 0);
            _positionableRepository.Update(positionable);
        }

        private void Cast(Actor actor)
        {
            if (actor.DesiredActions.Skill.HasValue == false)
            {
                return;
            }

            SkillId skillId = actor.DesiredActions.Skill.Value;
            UnitId caster = actor.Id;
            actor.DesireCast(default);
            _actorRepository.Update(actor);

            if (_skillOwnerRepository.TryGet(caster, out SkillOwner skillOwner) == false)
            {
                return;
            }

            if (skillOwner.GetCooldown(skillId) > 0)
            {
                return;
            }

            AbilityKey abilityKey = new(caster, skillId);
            Ability ability = _abilityFactory.Create(skillId, caster);

            if (CanCast(actor, ability.Flags, abilityKey) != CastFailReason.Success)
            {
                UnityEngine.Debug.Log("Can't cast - cast forbidden");
                return;
            }

            if (_skillExecutionPort.BeginCast(abilityKey) == false)
            {
                return;
            }

            if (ability.Actions != null && ability.Actions.Count > 0)
            {
                StartCastAction(actor, ability.Actions.First(), skillId, ability.Flags);
            }
        }

        private CastFailReason CanCast(Actor actor, SkillFlags skillFlags, AbilityKey abilityKey)
        {
            Action action = actor.CurrentAction;

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

        private void StartCastAction(Actor actor, ActionId actionId, SkillId skillId, SkillFlags skillFlags)
        {
            if (actor.CurrentAction != null)
            {
                Entities.Action oldAction = actor.CurrentAction;
                oldAction.Interrupt(InterruptReason.Chained);

                if (oldAction.TryGet(out IAbilityAction oldAbilityAction))
                {
                    _skillActionStateChangeHandler.Handle(new(actor.Id, oldAbilityAction.Source), ActionState.Inactive);
                }
            }

            actor.StartAction(_actionFactory.CreateCastAction(actionId, skillId, skillFlags));
            _actorRepository.Update(actor);
            _skillActionStateChangeHandler.Handle(new(actor.Id, skillId), ActionState.Startup);
        }

        private void StopAction(Actor actor)
        {
            actor.StopAction();
            _actorRepository.Update(actor);
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
