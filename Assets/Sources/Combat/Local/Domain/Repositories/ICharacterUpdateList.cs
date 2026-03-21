using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories
{
    public interface ICharacterUpdateList
    {
        void Create(Updatable value);
        void Update(Updatable value);
        Updatable Get(EntityId id);
        IReadOnlyCollection<Updatable> GetAll();
        void Delete(EntityId id);
    }
}
