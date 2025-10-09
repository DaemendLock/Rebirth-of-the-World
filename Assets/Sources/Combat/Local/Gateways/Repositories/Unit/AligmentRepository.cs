using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class AligmentRepository : IAligmentRepository
    {
        private readonly Dictionary<EntityId, Aligment> _values = new();

        public void Create(Aligment value) => _values.Add(value.Id, value);

        public void Delete(EntityId id) => _values.Remove(id);

        public Aligment Get(EntityId id) => _values[id];

        public void Update(Aligment value) => _values[value.Id] = value;
    }
}
