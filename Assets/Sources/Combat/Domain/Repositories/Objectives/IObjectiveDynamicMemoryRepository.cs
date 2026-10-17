using Combat.Common.Primitives;

using System;

namespace Combat.Domain.Repositories
{
    public interface IObjectiveDynamicMemoryRepository
    {
        void Create(ObjectiveId id, int size);
        void Delete(ObjectiveId id);
        void Save<T>(ObjectiveId id, T value) where T : unmanaged;
        bool TryGetRawData(ObjectiveId id, out ReadOnlySpan<byte> data);
    }
}
