using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.IDK;
using Combat.Local.Domain.API.Skills;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;

using UnityEngine;

namespace Testing.Local.Temp.Services
{
    public class HitHandlingService : IHitHandlingService
    {
        private readonly UnitApiRepository _unitApiRepository;

        private readonly IHitboxRepository _hitboxRepository;
        private readonly IHurtableRepository _hurtboxRepository;

        public HitHandlingService(UnitApiRepository unitApiRepository, IHitboxRepository hitboxRepository, IHurtableRepository hurtboxRepository)
        {
            _unitApiRepository = unitApiRepository;
            _hitboxRepository = hitboxRepository;
            _hurtboxRepository = hurtboxRepository;
        }

        public void HandleHit(HitboxId source, HurtboxId target, Vector3 position)
        {
            Hitbox hitbox = _hitboxRepository.Get(source);
            Hurtbox hurtbox = _hurtboxRepository.Get(target);

            HitRecord @event = new(_unitApiRepository.Get(hitbox.Owner), hitbox.Type, _unitApiRepository.Get(hurtbox.Owner), hurtbox.Type, position);

            UnityEngine.Debug.Log($"Handling hit - Hitbox: {hitbox.Id}({hitbox.Type} from {hitbox.Owner}); Hurtbox: {hurtbox.Id}({hurtbox.Type} of {hurtbox.Owner});");
            //IHitHandler[] handlers = GetHitHandlers(hitbox.Owner);

            //foreach (IHitHandler handler in handlers)
            //{
            //    if (handler.OnHit(@event) == false)
            //    {
            //        continue;
            //    }

            //    _hitboxRepository.Delete(source);
            //}
        }

        private IHitHandler[] GetHitHandlers(EntityId owner) => throw new System.NotImplementedException();
    }
}
