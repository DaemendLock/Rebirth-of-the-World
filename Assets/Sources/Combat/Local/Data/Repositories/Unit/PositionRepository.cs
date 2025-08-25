using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

namespace Combat.Server.Data.Repositories.Unit
{
    public class PositionRepository : IPositionRepository
    {
        private readonly Dictionary<EntityId, Transform> _values = new();

        public void Create(Transform value) => _values.Add(value.Id, value);

        public void Delete(EntityId id) => _values.Remove(id);

        public Transform Get(EntityId id) => _values[id];

        public void Update(Transform value) => _values[value.Id] = value;
    }
}
