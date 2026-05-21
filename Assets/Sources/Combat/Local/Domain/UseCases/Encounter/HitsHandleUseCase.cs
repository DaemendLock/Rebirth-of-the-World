using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.UseCases.Scene
{
    public class HitsHandleUseCase
    {
        private readonly IHitboxOwnerRepository _hitboxRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IAbilityRepository _skillRepository;

        public HitsHandleUseCase(IHitboxOwnerRepository hitboxRepository, IActorRepository actorRepository, IAbilityRepository skillRepository)
        {
            _hitboxRepository = hitboxRepository;
            _actorRepository = actorRepository;
            _skillRepository = skillRepository;
        }

        public void Execute(System.ReadOnlySpan<Updatable> targets)
        {
            foreach (var target in targets)
            {
                var val = _hitboxRepository.GetHits(target.Id);
                HandleRecord(target.Id, val);
            }
        }

        private void HandleRecord(UnitId target, IEnumerable<Queue<HitRecord>> values)
        {
            Actor actor = _actorRepository.Get(target);

            if (actor.CurrentAction == null)
            {
                return;
            }

            if (actor.CurrentAction.CurrentState != Common.ValueObjects.ActionState.Active)
            {
                return;
            }

            var properties = _skillRepository.GetPropertyContainer(new(actor.Id, actor.CurrentAction.Source));

            if (properties.TryGet(out ISkillHitStrategy hitEffect))
            {
                foreach (var value in values)
                {
                    while (value.TryDequeue(out var record))
                    {
                        hitEffect.HandleHit(record);
                    }
                }
            }
        }
    }
}
