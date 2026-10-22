using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IUnitStore
    {
        void Create(UnitContext context);
        ref UnitContext Get(UnitId id);
        void Delete(UnitId id);
    }
}
