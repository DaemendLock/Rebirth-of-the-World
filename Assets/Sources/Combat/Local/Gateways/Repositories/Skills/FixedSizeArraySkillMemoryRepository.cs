using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Skill;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Combat.Local.Gateways.Repositories.Skills
{
    public sealed class FixedSizeArraySkillMemoryRepository : ISkillDynamicMemoryRepository
    {
        private readonly Dictionary<AbilityKey, byte[]> _values = new();

        public void Create(AbilityKey key, int size) => _values.Add(key, new byte[size]);

        public void Delete(AbilityKey abilityKey) => _values.Remove(abilityKey);

        public void Save<T>(AbilityKey abilityKey, T value) where T : unmanaged
        {
            if (_values.TryGetValue(abilityKey, out var memory) == false)
            {
                memory = new byte[128];
                _values.Add(abilityKey, memory);
            }

            MemoryMarshal.Write(memory, ref value);
        }

        public bool TryGetRawData(AbilityKey abilityKey, out ReadOnlySpan<byte> result)
        {
            if (_values.TryGetValue(abilityKey, out var val) == false)
            {
                result = Span<byte>.Empty;
                return false;
            }

            result = val;
            return true;
        }
    }
}
