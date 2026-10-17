using Combat.Common.Primitives;
using Combat.Local.Application.DTO;

namespace Combat.Local.Application.Endpoints.Skills
{
    public interface ISkillHitHandler
    {
        void Reset(AbilityKey abilityKey);
        void HandleHit(AbilityKey abilityKey, HitRecord record);
    }
}
