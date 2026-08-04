using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Endpoints.Skills
{
    public interface ISkillHitHandler
    {
        void Reset();
        void HandleHit(HitRecord hitRecord);
    }
}
