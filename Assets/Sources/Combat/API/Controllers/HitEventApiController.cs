using Combat.API.DTO;
using Combat.API.Skills;
using Combat.Local.Events;

namespace Combat.API.Controllers
{
    public class HitEventApiController
    {
        private readonly CharacterApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;

        public HitEventApiController(SkillApiProvider skillApiProvider, CharacterApiProvider unitApiProvider)
        {
            _skillApiProvider = skillApiProvider;
            _unitApiProvider = unitApiProvider;
        }

        public void HandleHit(HitInfo hitInfo)
        {
            HitRecord hitRecord = CreateHitRecord(hitInfo);
            Handle(hitRecord);
        }

        private HitRecord CreateHitRecord(HitInfo hitInfo)
        {
            Unit hitboxOwner = _unitApiProvider.Get(hitInfo.Source);
            Unit hurtboxOwner = _unitApiProvider.Get(hitInfo.Target);
            SkillApi handler = hitInfo.Handler.HasValue ? _skillApiProvider.Get(new(hitInfo.Handler.Value.Value), hitInfo.Source) : null;
            return new(hitboxOwner, hitInfo.HitboxType, hurtboxOwner, hitInfo.HurtboxType, hitInfo.Location, handler);
        }

        private void Handle(HitRecord @event)
        {
            if (@event.Handler == null || @event.Handler.TryGetProperty(out IHitHandler handler) == false)
            {
                return;
            }

            handler.OnHit(@event);
        }
    }
}
