using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Repositories.Skill
{
    public interface ISkillDynamicMemoryRepository
    {
        void Create(AbilityKey abilityKey, int size);
        void Save<T>(AbilityKey abilityKey, T value) where T : unmanaged;
        bool TryGetRawData(AbilityKey abilityKey, out ReadOnlySpan<byte> result);
        void Delete(AbilityKey abilityKey);
    }
}
