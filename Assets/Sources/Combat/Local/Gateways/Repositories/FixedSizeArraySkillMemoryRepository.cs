using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Objectives;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Combat.Local.Gateways.Repositories
{
    public sealed class FixedSizeArrayObjectiveMemoryRepository : IObjectiveMemoryRepository
    {
        private readonly Dictionary<ObjectiveId, byte[]> _values = new();

        public void Create(ObjectiveId id, int size) => _values.Add(id, new byte[size]);

        public void Delete(ObjectiveId id) => _values.Remove(id);

        public void Save<T>(ObjectiveId id, T value) where T : unmanaged
        {
            if (_values.TryGetValue(id, out var memory) == false)
            {
                memory = new byte[128];
                _values.Add(id, memory);
            }

            MemoryMarshal.Write(memory, ref value);
        }

        public bool TryGetRawData(ObjectiveId id, out ReadOnlySpan<byte> result)
        {
            if (_values.TryGetValue(id, out var val) == false)
            {
                result = Span<byte>.Empty;
                return false;
            }

            result = val;
            return true;
        }
    }
}
