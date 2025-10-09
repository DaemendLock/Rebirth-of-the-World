using Combat.Common.ValueObjects;
using Combat.Local.Controllers.Inputs;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases.Scene;

using UnityEngine;

namespace Combat.Local.Controllers
{
    public class HitController
    {
        private readonly IHitboxRepository _hitboxRepository;
        private readonly IHurtableRepository _hurtableRepository;
        private readonly RecordHitUseCase _recordHitUseCase;

        private int _nextId = 0;

        public HitController(IHitboxRepository hitboxRepository, IHurtableRepository hurtableRepository, RecordHitUseCase recordHitUseCase)
        {
            _hitboxRepository = hitboxRepository;
            _hurtableRepository = hurtableRepository;
            _recordHitUseCase = recordHitUseCase;
        }

        public HitboxId CreateHitbox(Collider collider, HitboxType type, EntityId handler)
        {
            HitboxId hitboxId = new(_nextId++);
            _hitboxRepository.Create(new(hitboxId, type, handler));
            HitboxInputComponent hitbox = collider.gameObject.AddComponent<HitboxInputComponent>();
            hitbox.Id = hitboxId;
            hitbox.Hitted += RegisterHit;
            hitbox.Destroied += RemoveHitbox;

            return hitboxId;
        }

        public HurtboxId CreateHurtbox(Collider collider, HurtboxType type, EntityId owner)
        {
            HurtboxId hurtboxId = new(_nextId++);
            _hurtableRepository.Create(new(hurtboxId, type, owner));
            HurtboxInputComponent hurtbox = collider.gameObject.AddComponent<HurtboxInputComponent>();
            hurtbox.Id = hurtboxId;
            hurtbox.Destroied += RemoveHurtbox;

            return hurtboxId;
        }

        public void RemoveHitbox(HitboxId id) => _hitboxRepository.Delete(id);

        public void RemoveHurtbox(HurtboxId id) => _hurtableRepository.Delete(id);

        public void RegisterHit(HitboxId hitboxId, HurtboxId hurtboxId, Vector3 position) => _recordHitUseCase.Execute(hitboxId, hurtboxId, position);
    }
}