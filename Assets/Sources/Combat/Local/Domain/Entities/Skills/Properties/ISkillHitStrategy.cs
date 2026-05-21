using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Skills.Effects
{
    public interface ISkillHitStrategy
    {
        void Reset();
        void HandleHit(HitRecord hitRecord);
    }
}
