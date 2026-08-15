using Combat.Common.Primitives;
using Combat.Local.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Combat.Local.Gateways.Repositories
{
    public sealed class FixedSizeArrayStatusMemoryRepository : IStatusDynamicMemoryRepository
    {
        private readonly Dictionary<StatusId, byte[]> _values = new();

        public void Create(StatusId id, int size) => _values.Add(id, new byte[size]);

        public void Delete(StatusId id) => _values.Remove(id);

        public void Save<T>(StatusId id, T value) where T : unmanaged
        {
            if (_values.TryGetValue(id, out byte[] memory) == false)
            {
                memory = new byte[128];
                _values.Add(id, memory);
            }

            MemoryMarshal.Write(memory, ref value);
        }

        public bool TryGetRawData(StatusId id, out ReadOnlySpan<byte> result)
        {
            if (_values.TryGetValue(id, out byte[] value) == false)
            {
                result = ReadOnlySpan<byte>.Empty;
                return false;
            }

            result = value;
            return true;
        }
    }
}
