using Combat.Common.ValueObjects;

using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases.Scene
{
    public class RecordHitUseCase
    {
        private readonly IHitRecordRepository _hitRecordRepository;

        public RecordHitUseCase(IHitRecordRepository hitRecordRepository)
        {
            _hitRecordRepository = hitRecordRepository;
        }

        public void Execute(HitboxId hitboxId, HurtboxId hurtboxId, Vector3 location)
        {
            HitRecord hitRecord = new(hitboxId, hurtboxId, location);
            _hitRecordRepository.Register(hitRecord);
        }
    }

    public interface IHitRecordPresenter
    {
        void Present(HitRecord hitRecord);
    }
}
