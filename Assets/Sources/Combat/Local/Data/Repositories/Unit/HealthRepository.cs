using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Server.Data.Repositories.Unit
{
    public class HealthRepository : IHealthRepository
    {
        private readonly Dictionary<EntityId, Health> _values = new();

        public void Create(Health value) => _values.Add(value.Id, value);

        public void Delete(EntityId id) => _values.Remove(id);

        public Health Get(EntityId id) => _values[id];

        public void Update(Health value) => _values[value.Id] = value;

        public IReadOnlyCollection<Health> GetAll() => _values.Values;
    }
}
