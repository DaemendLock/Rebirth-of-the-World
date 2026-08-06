using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IHealthRepository
    {
        delegate void Processor(Health health);

        void Create(Health value);
        bool TryGet(UnitId id, out Health health);
        void Update(Health health);
        void Delete(UnitId id);
    }
}
