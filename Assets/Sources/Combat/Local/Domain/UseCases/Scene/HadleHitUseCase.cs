using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases.Scene
{
    public class HandleHitUseCase
    {
        private readonly IHitRecordRepository _hitRecordRepository;
        private readonly IHitboxRepository _hitboxRepository;
        private readonly IHurtableRepository _hurtboxRepository;
        private readonly IHitEventHandler _hitEventHandler;

        public HandleHitUseCase(IHitRecordRepository hitRecordRepository, IHitboxRepository hitboxRepository, IHurtableRepository hurtboxRepository, IHitEventHandler hitEventHandler)
        {
            _hitRecordRepository = hitRecordRepository;
            _hitboxRepository = hitboxRepository;
            _hurtboxRepository = hurtboxRepository;
            _hitEventHandler = hitEventHandler;
        }

        public void Execute()
        {
            while (_hitRecordRepository.TryPop(out HitRecord value))
            {
                Hitbox hitbox = _hitboxRepository.Get(value.HitboxId);
                Hurtbox hurtbox = _hurtboxRepository.Get(value.HurtboxId);

                _hitEventHandler.HandleEvent(hitbox, hurtbox, value.Location);
            }
        }
    }

    public interface IHitEventHandler
    {
        void HandleEvent(Hitbox hitbox, Hurtbox hurt, Vector3 position);
    }
}
