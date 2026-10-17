using Combat.Common.Primitives;

using System;

namespace Combat.Domain.Repositories
{
    public interface IStatusDynamicMemoryRepository
    {
        void Create(StatusId abilityKey, int size);
        void Save<T>(StatusId abilityKey, T value) where T : unmanaged;
        bool TryGetRawData(StatusId abilityKey, out ReadOnlySpan<byte> result);
        void Delete(StatusId abilityKey);
    }
}
