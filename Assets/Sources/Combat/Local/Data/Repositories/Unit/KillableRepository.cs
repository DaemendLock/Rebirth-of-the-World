using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

namespace Combat.Server.Data.Repositories.Unit
{
    public class KillableRepository : IKillableRepository
    {
        private readonly Dictionary<EntityId, Killable> _values = new();

        public void Create(Killable value) => _values.Add(value.Id, value);

        public void Delete(EntityId id) => _values.Remove(id);

        public Killable Get(EntityId id) => _values[id];

        public void Update(Killable value) => _values[value.Id] = value;
    }
}
