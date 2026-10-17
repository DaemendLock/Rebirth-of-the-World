using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Domain.Repositories
{
    public interface IHealthRepository
    {
        void Create(HealthOwner value);
        ref HealthOwner Get(UnitId id);
        void Delete(UnitId id);
    }
}
