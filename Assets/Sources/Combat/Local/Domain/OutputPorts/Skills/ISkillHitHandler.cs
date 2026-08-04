using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Endpoints.Skills
{
    public interface ISkillHitHandler
    {
        void Reset(AbilityKey abilityKey);
        void HandleHits(AbilityKey abilityKey, Queue<HitRecord> hitRecords);
    }
}
