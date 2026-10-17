using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly Dictionary<UnitId, AttributesOwner> _values;

        public AttributesRepository()
        {
            _values = new();
        }

        public void Create(AttributesOwner value) => _values.Add(value.Id, value);

        public AttributesOwner Get(UnitId id) => _values[id];

        public void Delete(UnitId id) => _values.Remove(id);
    }
}
