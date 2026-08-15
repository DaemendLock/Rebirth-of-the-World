using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IHealthRepository
    {
        void Create(HealthOwner value);
        bool TryGet(UnitId id, out HealthOwner health);
        void Update(HealthOwner health);
        void Delete(UnitId id);
    }
}
