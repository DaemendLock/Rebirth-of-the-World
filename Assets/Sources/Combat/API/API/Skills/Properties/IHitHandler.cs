using Combat.API.DTO;

namespace Combat.API.Skills
{
    public interface IHitHandler : ISkillProperty
    {
        bool OnHit(HitRecord @event);
    }
}
