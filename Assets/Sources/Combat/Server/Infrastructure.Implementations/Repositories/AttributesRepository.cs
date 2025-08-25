using System.Collections.Generic;

using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.Implementations.Attributes;
using Server.Combat.Domain.Units.ValueObjects;
using Server.Combat.Infrastructure.Repositories;

namespace Server.Combat.Infrastructure.Implementations.Repositories
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly Dictionary<int, AttributeCollection> _values;

        public AttributesRepository()
        {
            _values = new();
        }

        public IAttributeCollection<Attribute> Get(EntityId id)
        {
            if (_values.TryGetValue(id.Value, out var value) == false)
            {
                return null;
            }

            return value.Value;
        }

        public IAttributeCollection<Attribute> Create(EntityId id, IAttributeCollection<Attribute> defaultValue)
        {
            if (_values.ContainsKey(id.Value))
            {
                throw new System.InvalidOperationException("Can not allocate attribute collection with overlaping id.");
            }

            AttributeCollection attributeCollection = new(defaultValue);
            _values[id.Value] = attributeCollection;

            return attributeCollection.Value;
        }

        public void Remove(EntityId id) => _values.Remove(id.Value);

        public void Reset()
        {
            foreach (AttributeCollection attributeCollection in _values.Values)
            {
                attributeCollection.Reset();
            }
        }

        public void Update(EntityId id, IAttributeCollection<Attribute> attribute)
        {

        }

        private readonly struct AttributeCollection
        {
            private readonly StatsTable _attributes;
            private readonly System.Memory<AttributeValue> _defaultValue;
            private readonly StatsTable _defaultStatsTable;

            public AttributeCollection(IAttributeCollection<Attribute> defaultValue)
            {
                _defaultStatsTable = new StatsTable(defaultValue);
                _defaultValue = _defaultStatsTable.Memory;
                _attributes = new();
            }

            public IAttributeCollection<Attribute> Value => _attributes;

            public void Reset()
            {
                _attributes.Populate(_defaultValue);
            }
        }
    }
}
