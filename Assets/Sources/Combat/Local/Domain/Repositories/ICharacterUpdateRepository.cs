using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories
{
    public interface ICharacterUpdateRepository
    {
        void Create(Updatable value);
        void Update(Updatable value);
        Updatable Get(UnitId id);
        bool TryGet(UnitId id, out Updatable updatable);
        IReadOnlyCollection<Updatable> GetAll();
        void Delete(UnitId id);
    }
}
