using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IAttributesRepository
    {
        void Create(Attributes attributes);
        void Update(Attributes attributes);
        Attributes Get(EntityId entityId);
        void Delete(EntityId id);

        IEnumerable<Attributes> GetAll();
    }
}
