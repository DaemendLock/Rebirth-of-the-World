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
        private readonly IAbilityRepository _abilityRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly ActionFactory _actionFactory;

        private readonly ISkillExecutionPort _skillExecutionPort;
        private readonly ISkillActionStateChangeHandler _skillActionStateChangeHandler;
        private readonly ISkillHitHandler _skillHitHandler;

        public ActorActAllUseCase(IAttributesRepository attributesRepository, IPositionableRepository positionableRepository, IActorRepository actorRepository, IAbilityRepository skillRepository, ActionFactory actionFactory, ISkillHitHandler skillHitHandler)
        {
            _attributesRepository = attributesRepository;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;
            _abilityRepository = skillRepository;
            _actionFactory = actionFactory;
            _skillHitHandler = skillHitHandler;
        }

        public void Execute(System.ReadOnlySpan<Updatable> targets, float deltaTime)
        {
            foreach (Updatable target in targets)
            {
                ExecuteFor(target.Id, deltaTime * target.TimeScale);
            }
        }

        private void ExecuteFor(UnitId target, float deltaTime)
        {
            Actor actor = _actorRepository.Get(target);
            AttributesOwner attributesOwner = _attributesRepository.Get(target);

            UpdateAction(actor, deltaTime);
            actor = _actorRepository.Get(target);
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
                _skillActionStateChangeHandler.Handle(new(actor.Id, action.Source), ActionState.Inactive);
                return;
            }

            ActionState actionState = action.CurrentState;

            if (actionState == ActionState.Inactive)
            {
                StopAction(actor);
                return;
            }

            action.Progress(deltaTime);
            _actorRepository.Update(actor);

            HandleStateChanges(actor.Id, action, actionState);
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

            float speed = attributesOwner.GetAttributeValue(Attribute.Speed);

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
            SkillOwner skillOwner = _skillOwnerRepository.Get(caster);

            if (skillOwner.GetCooldown(skillId) > 0)
            {
                return;
            }

            AbilityKey abilityKey = new(caster, skillId);
            Ability skill = _abilityRepository.Get(abilityKey);

            if (CanCast(actor, skill.Flags, abilityKey) == false)
            {
                UnityEngine.Debug.Log("Can't cast - cast forbidden");
                return;
            }

            bool requireAction = _skillExecutionPort.BeginCast(abilityKey);
            //skill.StartCooldown(10);
            _abilityRepository.Update(skill);

            if (requireAction)
            {
                return;
            }

            ActionId actionId = skill.Actions.First();
            StartCastAction(actor, actionId, skill);
        }

        private bool CanCast(Actor actor, SkillFlags skillFlags, AbilityKey abilityKey)
        {
            Action action = actor.CurrentAction;

            if (skillFlags.HasFlag(Common.Flags.SkillFlags.Instant) || action == null)
            {
                return _skillExecutionPort.CanCast(abilityKey) == CastFailReason.Success;
            }

            if (action.CurrentState == ActionState.Recovery)
            {
                if (action.CanChainInto(abilityKey.Skill) == false)
                {
                    return false;
                }
            }
            else if (action.CurrentState != ActionState.Inactive)
            {
                return false;
            }

            return _skillExecutionPort.CanCast(abilityKey) == CastFailReason.Success;
        }

        private void StartCastAction(Actor actor, ActionId actionId, Ability source)
        {
            if (actor.CurrentAction != null)
            {
                Entities.Action oldAction = actor.CurrentAction;
                oldAction.Interrupt(InterruptReason.Chained);
                _skillActionStateChangeHandler.Handle(new(actor.Id, oldAction.Source), ActionState.Inactive);
            }

            actor.StartAction(_actionFactory.CreateCastAction(actionId, source));
            _actorRepository.Update(actor);
            _skillActionStateChangeHandler.Handle(new(actor.Id, source.SkillId), ActionState.Startup);
        }

        private void StopAction(Actor actor)
        {
            actor.StopAction();
            _actorRepository.Update(actor);
        }

        private void HandleStateChanges(UnitId actorId, Action action, ActionState oldState)
        {
            if (action.CurrentState == oldState)
            {
                return;
            }

            AbilityKey key = new(actorId, action.Source);
            _skillHitHandler.Reset(key);

            _skillActionStateChangeHandler.Handle(key, action.CurrentState);
        }
    }
}
