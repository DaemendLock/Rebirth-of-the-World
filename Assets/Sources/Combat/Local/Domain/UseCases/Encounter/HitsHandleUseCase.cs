using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Entities;
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
        private readonly ISkillHitHandler _skillHitHandler;

        public HitsHandleUseCase(IHitboxOwnerRepository hitboxRepository, IActorRepository actorRepository)
        {
            _hitboxRepository = hitboxRepository;
            _actorRepository = actorRepository;
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

            foreach (var value in values)
            {
                _skillHitHandler.HandleHits(new(actor.Id, actor.CurrentAction.Source), value);
            }
        }
    }
}
