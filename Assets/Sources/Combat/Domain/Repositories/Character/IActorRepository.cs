using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Local.Application.Repositories
{
    public interface IActorRepository
    {
        void Create(Actor actor);
        bool TryGet(UnitId id, out Actor actor);
        void Update(Actor value);
        void Delete(UnitId id);
    }
}
