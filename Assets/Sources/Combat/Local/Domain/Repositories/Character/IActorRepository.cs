using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories
{
    public interface IActorRepository
    {
        void Create(Actor actor);
        Actor Get(EntityId id);
        void Update(Actor value);
        void Delete(EntityId id);

        ICollection<EntityId> GetAll();
    }
}
