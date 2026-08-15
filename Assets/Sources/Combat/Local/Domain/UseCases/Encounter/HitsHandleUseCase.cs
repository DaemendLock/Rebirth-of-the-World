using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases.Scene
{
    public sealed class HitRecordUseCase
    {
        private readonly IHitRecordQueue _hitRecordQueue;

        public void Execute(HitRecord hitRecord)
        {
            _hitRecordQueue.Enqueue(hitRecord);
        }
    }

    public class HitsHandleUseCase
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

            if (actor.CurrentAction == null)
            {
                return;
            }

            if (actor.CurrentAction.TryGet(out IAbilityAction abilityAction) == false ||
                abilityAction.State != Common.ValueObjects.ActionState.Active)
            {
                return;
            }

            _skillHitHandler.HandleHit(new(actor.Id, abilityAction.Source), record);
        }
    }
}
