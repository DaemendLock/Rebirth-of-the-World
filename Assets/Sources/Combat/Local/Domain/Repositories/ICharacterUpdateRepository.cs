using Combat.Common.Primitives;
using Combat.Local.Domain.Entities.Units;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories
{
    public interface ICharacterDeleteQueue
    {
        void Enqueue(UnitId unitId);
        bool TryDequeue(out UnitId unitId);
    }

    public interface ICharacterUpdateRepository
    {
        void Create(Updatable value);
        void Update(Updatable value);
        Updatable Get(UnitId id);
        bool TryGet(UnitId id, out Updatable updatable);
        IEnumerable<Updatable> GetAll();
        void Delete(UnitId id);
    }
}
