using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Repositories
{
    public interface IHealthRepository
    {
        delegate void Processor(Health health);

        Health Create(UnitId index, HealthValue health);
        bool TryGet(UnitId id, out Health health);
        void Update(UnitId id, Health health);
        void Delete(UnitId id);
    }
}
