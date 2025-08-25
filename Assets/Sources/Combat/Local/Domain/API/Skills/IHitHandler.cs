using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.DTO;

namespace Combat.Local.Domain.API.Skills
{
    public interface IHitHandler
    {
        bool CanHandle(HitboxType hitboxType);

        bool OnHit(HitRecord @event);
    }
}
