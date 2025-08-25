using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IPositionRepository
    {
        Transform Get(EntityId id);
        void Create(Transform value);
        void Update(Transform value);
        void Delete(EntityId id);
    }
}
