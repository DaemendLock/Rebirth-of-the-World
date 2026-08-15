using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IHurtableRepository
    {
        void Create(UnitId owner);
        void Delete(UnitId id);
    }
}
