using System.Collections.Generic;
using System.Linq;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly Dictionary<EntityId, Attributes> _baseValues;
        private readonly Dictionary<EntityId, Attributes> _values;

        public AttributesRepository()
        {
            _baseValues = new();
            _values = new();
        }

        public void Create(Attributes attributes)
        {
            _baseValues.Add(attributes.Id, attributes);
            _values.Add(attributes.Id, attributes);
        }

        public Attributes Get(EntityId id) => _values[id];

        public void Update(Attributes attributes)
        {
            _values[attributes.Id] = attributes;
        }

        public void Delete(EntityId id)
        {
            _baseValues.Remove(id);
            _values.Remove(id);
        }

        public IEnumerable<Attributes> GetAll() => _baseValues.Values.ToArray();
    }
}
