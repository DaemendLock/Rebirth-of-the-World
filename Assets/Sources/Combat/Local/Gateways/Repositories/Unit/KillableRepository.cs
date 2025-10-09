using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class KillableRepository : IKillableRepository
    {
        private readonly Dictionary<EntityId, bool> _values = new();

        public void Create(Killable value) => _values.Add(value.Id, value.Alive);

        public void Delete(EntityId id) => _values.Remove(id);

        public Killable Get(EntityId id) => new(id, _values[id]);

        public void Update(Killable value) => _values[value.Id] = value.Alive;
    }
}
