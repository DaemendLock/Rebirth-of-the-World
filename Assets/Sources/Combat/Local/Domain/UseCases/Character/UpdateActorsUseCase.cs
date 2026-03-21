using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct UpdateActorsUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly ISkillRepository _skillRepository;

        public UpdateActorsUseCase(IActorRepository actorRepository, ISkillRepository skillRepository)
        {
            _actorRepository = actorRepository;
            _skillRepository = skillRepository;
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
            IAction action = actor.CurrentAction;

            if (action == null)
            {
                return;
            }

            if (action.IsActive == false)
            {
                actor.CurrentAction = null;
                _actorRepository.Update(actor);
                return;
            }

            ActionState actionState = action.CurrentState;

            float activeTime = action.ActiveTime + deltaTime;
            float effectiveTime = action.EffectiveTime;

            if (!action.Flags.HasFlag(ActionFlags.Holdable) || actionState != ActionState.Active)
            {
                effectiveTime += deltaTime;
            }

            ActionData data = new(activeTime, effectiveTime);

            action.Update(data);

            if (action.CurrentState == actionState)
            {
                return;
            }

            if (action.CurrentState == ActionState.Active)
            {
                action.HittedTargets.Clear();
            }

            Skill skill = _skillRepository.Get(action.Source, actor.Id);

            if (skill.TryGetEffect(out SkillActionStateChangeEffect effect))
            {
                effect.Handle(action.CurrentState);
            }
        }
    }
}
