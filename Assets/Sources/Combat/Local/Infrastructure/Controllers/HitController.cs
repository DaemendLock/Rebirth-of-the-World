using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;
using Combat.Local.Presentation.Units.ViewModels;

using UnityEngine;

namespace Combat.Local.Infrastructure.Controllers
{
    public class HitController
    {
        private readonly IHitboxRepository _hitboxRepository;
        private readonly IHurtableRepository _hurtableRepository;
        private readonly IModelUpdateService _updateService;

        public HitController(IHitboxRepository hitboxRepository, IModelUpdateService updateService, IHurtableRepository hurtableRepository)
        {
            _hitboxRepository = hitboxRepository;
            _updateService = updateService;
            _hurtableRepository = hurtableRepository;
        }

        public void AddHitbox(HitboxViewModel hitbox, EntityId owner)
        {
            _hitboxRepository.Create(new(hitbox.Id, hitbox.Type, owner));
            hitbox.Hitted += RegisterHit;
        }

        public void RemoveHitbox(HitboxId hitboxId)
        {
            _hitboxRepository.Delete(hitboxId);
        }

        public void AddHurtbox(HurtboxViewModel hurtbox, EntityId owner) => _hurtableRepository.Create(new(hurtbox.Id, hurtbox.Type, owner));

        public void RemoveHurtbox(HurtboxId id) => _hurtableRepository.Delete(id);

        public void RegisterHit(HitboxViewModel hitbox, HurtboxViewModel hurtbox, Vector3 position) => _updateService.RegisterHit(hitbox.Id, hurtbox.Id, position);
    }
}