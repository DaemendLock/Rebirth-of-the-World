using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Local.Application.Repositories
{
    public interface IPositionableRepository
    {
        void Create(Positionable positionable);
        void Delete(UnitId entityId);
    }
}
