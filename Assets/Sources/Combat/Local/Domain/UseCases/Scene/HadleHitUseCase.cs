using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases.Scene
{
    public class HandleHitsUseCase
    {
        private readonly IHitRecordRepository _hitRecordRepository;
        private readonly IHitboxRepository _hitboxRepository;
        private readonly IHurtableRepository _hurtboxRepository;
        private readonly IHitEventHandler _hitEventHandler;
        private readonly IActorRepository _actorRepository;

        public HandleHitsUseCase(IHitRecordRepository hitRecordRepository, IHitboxRepository hitboxRepository, IHurtableRepository hurtboxRepository, IHitEventHandler hitEventHandler, IActorRepository actorRepository)
        {
            _hitRecordRepository = hitRecordRepository;
            _hitboxRepository = hitboxRepository;
            _hurtboxRepository = hurtboxRepository;
            _hitEventHandler = hitEventHandler;
            _actorRepository = actorRepository;
        }

        public void Execute()
        {
            while (_hitRecordRepository.TryPop(out HitRecord value))
            {
                HandleRecord(value);
            }
        }

        private void HandleRecord(HitRecord value)
        {
            Hitbox hitbox = _hitboxRepository.Get(value.HitboxId);
            Hurtbox hurtbox = _hurtboxRepository.Get(value.HurtboxId);
            Actor actor = _actorRepository.Get(hitbox.Owner);

            if (actor.CurrentAction == null)
            {
                return;
            }

            if (actor.CurrentAction.CurrentState != Common.ValueObjects.ActionState.Active)
            {
                return;
            }

            if (actor.CurrentAction.HittedTargets.Contains(hurtbox.Owner))
            {
                return;
            }

            actor.CurrentAction.HittedTargets.Add(hurtbox.Owner);

            _hitEventHandler.HandleEvent(hitbox, hurtbox, value.Location);
        }
    }

    public interface IHitEventHandler
    {
        void HandleEvent(Hitbox hitbox, Hurtbox hurt, Vector3 position);
    }
}
