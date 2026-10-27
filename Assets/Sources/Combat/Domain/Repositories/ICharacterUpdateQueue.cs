using Combat.Common.Primitives;

using System.Collections.Generic;

namespace Combat.Local.Application.Repositories
{
    public interface ICharacterDeleteQueue
    {
        void Enqueue(UnitId unitId);
        bool TryDequeue(out UnitId unitId);
    }

    public interface ICharacterUpdateQueue
    {
        void Add(UnitId value);
        IReadOnlyCollection<UnitId> GetAll();
        void Delete(UnitId id);
    }
}
