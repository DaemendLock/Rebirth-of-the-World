using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IHealthRepository
    {
        delegate void Processor(Health health);

        Health Create(UnitId id);
        Health Get(UnitId id);
        void Update(Health health);
        void Delete(UnitId id);
    }
}
