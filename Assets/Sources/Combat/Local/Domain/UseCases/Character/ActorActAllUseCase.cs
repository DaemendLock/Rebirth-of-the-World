using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System.Linq;

using UnityEngine;

namespace Combat.Local.Domain.UseCases.Character
{
    public sealed class ActorActAllUseCase
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IAbilityRepository _abilityRepository;
        private readonly ActionFactory _actionFactory;

        public ActorActAllUseCase(IAttributesRepository attributesRepository, IPositionableRepository positionableRepository, IActorRepository actorRepository, IAbilityRepository skillRepository, ActionFactory actionFactory)
        {
            _attributesRepository = attributesRepository;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;
            _abilityRepository = skillRepository;
            _actionFactory = actionFactory;
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
                IAbilityPropertyContainer properties = _abilityRepository.GetPropertyContainer(new(actor.Id, action.Source));
                StopAction(actor);

                if (properties.TryGet(out SkillActionStateChangeEffect effect))
                {
                    effect.Handle(ActionState.Inactive);
                }

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
            Vector2 relativeDirection = new(actor.DesiredActions.MovementDirection.X, actor.DesiredActions.MovementDirection.Y);

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

            Vector3 oldSpeed = positionable.Velocity;
            positionable.Velocity = (positionable.Rotation * new Vector3(relativeDirection.x, 0, relativeDirection.y) * speed) + new Vector3(0, oldSpeed.y, 0);
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
            Ability skill = _abilityRepository.Get(new(caster, skillId));

            if (skill.ActiveCooldown > 0)
            {
                return;
            }

            if (skill.Properties.TryGet(out SkillCastEffect effect) == false)
            {
                Debug.Log("Can't cast - no castable component assigned");
                return;
            }

            if (CanCast(actor, skill, effect) == false)
            {
                Debug.Log("Can't cast - cast forbidden");
                return;
            }

            effect.Execute();
            //skill.StartCooldown(10);
            _abilityRepository.Update(skill);

            if (skill.Flags.HasFlag(Common.Flags.SkillFlags.Instant))
            {
                return;
            }

            ActionId actionId = skill.Actions.First();
            StartCastAction(actor, actionId, skill);
        }

        private bool CanCast(Actor actor, Ability skill, SkillCastEffect effect)
        {
            Action action = actor.CurrentAction;

            if (skill.Flags.HasFlag(Common.Flags.SkillFlags.Instant) || action == null)
            {
                return effect.CanCast() == CastFailReason.Success;
            }

            if (action.CurrentState == ActionState.Recovery)
            {
                if (action.CanChainInto(skill.SkillId) == false)
                {
                    return false;
                }
            }
            else if (action.CurrentState != ActionState.Inactive)
            {
                return false;
            }

            return effect.CanCast() == CastFailReason.Success;
        }

        private void StartCastAction(Actor actor, ActionId actionId, Ability source)
        {
            if (source.Properties.TryGet(out SkillActionStateChangeEffect effect) == false)
            {
                return;
            }

            if (actor.CurrentAction != null)
            {
                Entities.Action oldAction = actor.CurrentAction;
                oldAction.Interrupt(InterruptReason.Chained);
                IAbilityPropertyContainer oldProperties = _abilityRepository.GetPropertyContainer(new(actor.Id, oldAction.Source));

                if (oldProperties.TryGet(out SkillActionStateChangeEffect oldEffect))
                {
                    oldEffect.Handle(ActionState.Inactive);
                }
            }

            actor.StartAction(_actionFactory.CreateCastAction(actionId, source));
            _actorRepository.Update(actor);
            effect.Handle(ActionState.Startup);
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

            var skill = _abilityRepository.GetPropertyContainer(new(actorId, action.Source));

            if (skill.TryGet(out ISkillHitStrategy hitEffect))
            {
                hitEffect.Reset();
            }

            if (skill.TryGet(out SkillActionStateChangeEffect effect))
            {
                effect.Handle(action.CurrentState);
            }
        }
    }
}
