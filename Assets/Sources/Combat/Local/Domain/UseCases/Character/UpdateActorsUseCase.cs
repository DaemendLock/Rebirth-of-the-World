using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct UpdateActorsUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IActionOutput _actionOutput;

        public UpdateActorsUseCase(IActorRepository actorRepository, ISkillRepository skillRepository, IActionOutput actionOutput)
        {
            _actorRepository = actorRepository;
            _skillRepository = skillRepository;
            _actionOutput = actionOutput;
        }

        public void Execute(float deltaTime, IReadOnlyCollection<Updatable> targets)
        {
            foreach (Updatable target in targets)
            {
                Actor actor = _actorRepository.Get(target.Id);

                UpdateActor(actor, deltaTime * target.TimeScale);
            }
        }

        private void UpdateActor(Actor actor, float deltaTime)
        {
            Entities.Action action = actor.CurrentAction;

            if (action == null)
            {
                return;
            }

            ActionState actionState = action.CurrentState;

            if (actionState == ActionState.Inactive)
            {
                StopAction(actor);
                return;
            }

            action.Update(deltaTime);
            actor.CurrentAction = action;
            _actorRepository.Update(actor);

            HandleStateChanges(actor.Id, action, actionState);
        }

        private void HandleStateChanges(EntityId actorId, Entities.Action action, ActionState oldState)
        {
            if (action.CurrentState == oldState)
            {
                return;
            }

            if (action.CurrentState == ActionState.Active)
            {
                action.HittedTargets.Clear();
            }

            Skill skill = _skillRepository.Get(action.Source, actorId);

            if (skill.TryGetEffect(out SkillActionStateChangeEffect effect))
            {
                effect.Handle(action.CurrentState);
            }
        }

        private void StopAction(Actor actor)
        {
            actor.CurrentAction = null;
            _actorRepository.Update(actor);
            _actionOutput.Present(actor);
        }
    }
}
