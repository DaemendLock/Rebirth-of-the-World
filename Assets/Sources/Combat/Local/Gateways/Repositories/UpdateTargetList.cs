using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Data.Repositories
{
    public class UpdateTargetList : ICharacterUpdateList
    {
        private readonly Dictionary<EntityId, Updatable> _values;

        public UpdateTargetList()
        {
            _values = new();
        }

        public void Create(Updatable value) => _values.Add(value.Id, value);

        public void Update(Updatable value) => _values[value.Id] = value;

        public Updatable Get(EntityId id) => _values[id];

        public IReadOnlyCollection<Updatable> GetAll() => _values.Values;

        public void Delete(EntityId id) => _values.Remove(id);
    }
}
