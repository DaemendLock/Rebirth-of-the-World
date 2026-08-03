using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Repositories.Skill
{
    public interface ISkillMemoryRepository
    {
        void Create(AbilityKey abilityKey, int size);
        void Save<T>(AbilityKey abilityKey, T value) where T : unmanaged;
        bool TryGetRawData(AbilityKey abilityKey, out Span<byte> result);
        void Delete(AbilityKey abilityKey);
    }
}
