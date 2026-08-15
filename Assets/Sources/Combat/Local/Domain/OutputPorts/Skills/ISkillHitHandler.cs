using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Endpoints.Skills
{
    public interface ISkillHitHandler
    {
        void Reset(AbilityKey abilityKey);
        void HandleHit(AbilityKey abilityKey, HitRecord record);
    }
}
