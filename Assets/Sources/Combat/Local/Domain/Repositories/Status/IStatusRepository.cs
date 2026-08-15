using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System;

namespace Combat.Local.Domain.Repositories
{
    public interface IStatusRepository
    {
        void Create(Status effect);
        void Update(Status effect);
        void Delete(StatusId effect);
        bool TryGet(StatusId id, out Status effect);
    }

    public interface IStatusDynamicMemoryRepository
    {
        void Create(StatusId abilityKey, int size);
        void Save<T>(StatusId abilityKey, T value) where T : unmanaged;
        bool TryGetRawData(StatusId abilityKey, out ReadOnlySpan<byte> result);
        void Delete(StatusId abilityKey);
    }
}
