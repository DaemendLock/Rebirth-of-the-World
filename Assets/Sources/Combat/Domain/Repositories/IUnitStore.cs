using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Domain.Repositories
{
    public interface IUnitStore
    {
        void Create(UnitContext context);
        UnitContext Get(UnitId id);
        bool TryGet(UnitId id, out UnitContext unit);
        void Delete(UnitId id);
    }
}
