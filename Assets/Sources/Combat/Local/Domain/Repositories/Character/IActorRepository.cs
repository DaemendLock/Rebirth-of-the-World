using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IActorRepository
    {
        void Create(Actor actor);
        Actor Get(UnitId id);
        void Update(Actor value);
        void Delete(UnitId id);
    }
}
