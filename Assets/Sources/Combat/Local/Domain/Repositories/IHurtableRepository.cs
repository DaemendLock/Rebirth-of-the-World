using Combat.Common.Primitives;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IHurtableRepository
    {
        void Create(UnitId owner);
        void Delete(UnitId id);
    }
}
