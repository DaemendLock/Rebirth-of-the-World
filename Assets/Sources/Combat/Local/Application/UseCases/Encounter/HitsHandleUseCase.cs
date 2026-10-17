using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Application.UseCases.Scene
{
    public sealed class HitsHandleUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly ISkillHitHandler _skillHitHandler;

        public HitsHandleUseCase(IActorRepository actorRepository, ISkillHitHandler skillHitHandler)
        {
            _actorRepository = actorRepository;
            _skillHitHandler = skillHitHandler;
        }

        public void Execute(HitRecord record)
        {
            UnitId target = record.HitboxOwner;

            if (_actorRepository.TryGet(target, out Actor actor) == false)
            {
                return;
            }

            if (_actorRepository.TryGet(record.HurtboxOwner, out _) == false)
            {
                return;
            }

            if (actor.CurrentAction == null)
            {
                return;
            }

            if (actor.CurrentAction.TryGet(out IAbilityAction currentAction) == false ||
                currentAction.State != ActionState.Active)
            {
                return;
            }

            _skillHitHandler.HandleHit(new(actor.Id, currentAction.Source), record);
        }
    }
}
