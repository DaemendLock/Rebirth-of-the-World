using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using Unity.Collections;

namespace Combat.Local.Domain.Repositories
{
    public interface IHealthRepository
    {
        Health Get(EntityId id);
        void Create(Health health);
        void Update(Health health);
        void Delete(EntityId id);

        IReadOnlyCollection<Health> GetAll();
    }
}
