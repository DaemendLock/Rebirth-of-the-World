using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IHealthRepository
    {
        Health Get(EntityId id);
        void Create(Health health);
        void Update(Health health);
        void Delete(EntityId id);
    }
}
