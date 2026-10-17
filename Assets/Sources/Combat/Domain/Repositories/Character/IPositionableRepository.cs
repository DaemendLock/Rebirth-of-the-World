using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Local.Application.Repositories
{
    public interface IPositionableRepository
    {
        void Create(Positionable positionable);
        void Update(Positionable positionable);
        Positionable Get(UnitId entityId);
        void Delete(UnitId entityId);
    }
}
