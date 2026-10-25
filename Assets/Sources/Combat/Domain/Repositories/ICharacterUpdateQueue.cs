using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;

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
        void Create(Updatable value);
        void Update(Updatable value);
        IEnumerable<Updatable> GetAll();
        void Delete(UnitId id);
    }
}
