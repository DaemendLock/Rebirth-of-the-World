using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IPositionableRepository
    {
        void Create(Positionable positionable);
        void Update(Positionable positionable);
        Positionable Get(EntityId entityId);
        void Delete(EntityId entityId);
    }
}
