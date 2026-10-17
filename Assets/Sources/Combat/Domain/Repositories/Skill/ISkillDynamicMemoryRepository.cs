using Combat.Common.Primitives;

using System;

namespace Combat.Domain.Repositories
{
    public interface ISkillDynamicMemoryRepository
    {
        void Create(AbilityKey abilityKey, int size);
        void Save<T>(AbilityKey abilityKey, T value) where T : unmanaged;
        bool TryGetRawData(AbilityKey abilityKey, out ReadOnlySpan<byte> result);
        void Delete(AbilityKey abilityKey);
    }
}
