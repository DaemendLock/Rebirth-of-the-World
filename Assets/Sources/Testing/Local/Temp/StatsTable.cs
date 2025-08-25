using System;
using System.Collections.Generic;

using Combat.Local.Domain.OldAttributes;
using Combat.Local.Domain.ValueObjects;

namespace Temp.Domain.Implementations
{
    [System.Serializable]
    public class StatsTable : IAttributeCollection<Combat.Local.Domain.ValueObjects.Attribute>
    {
        public const int AttributesCount = (int) Combat.Local.Domain.ValueObjects.Attribute.PARRY + 1;

        public static StatsTable UnitDefault => new(new AttributeValue[]
        {
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
            new(0, 100),
        });

        private readonly AttributeValue[] _values;

        public StatsTable()
        {
            _values = new AttributeValue[AttributesCount];
        }

        public StatsTable(IAttributeCollection<Combat.Local.Domain.ValueObjects.Attribute> attributeCollection) : this()
        {
            Add(attributeCollection);
        }

        private StatsTable(AttributeValue[] values)
        {
            _values = new AttributeValue[AttributesCount];
            values.AsSpan().CopyTo(_values);
        }

        public System.Memory<AttributeValue> Memory => _values.AsMemory();

        public void Add(IAttributeCollection<Combat.Local.Domain.ValueObjects.Attribute> table)
        {
            for (int i = 0; i < AttributesCount; i++)
            {
                _values[i] += table[(Combat.Local.Domain.ValueObjects.Attribute) i];
            }
        }

        public void Add(StatsTable table)
        {
            for (int i = 0; i < AttributesCount; i++)
            {
                _values[i] += table._values[i];
            }
        }

        public void Populate(System.Memory<AttributeValue> values)
        {
            values.Span.CopyTo(_values);
        }

        public void CopyTo(StatsTable target)
        {
            _values.AsSpan().CopyTo(target._values);
        }

        public void Clear()
        {
            _values.AsSpan().Clear();
        }

        public Dictionary<Combat.Local.Domain.ValueObjects.Attribute, AttributeValue> ToDictionary()
        {
            Dictionary<Combat.Local.Domain.ValueObjects.Attribute, AttributeValue> result = new();

            for (int i = 0; i < _values.Length; i++)
            {
                result[(Combat.Local.Domain.ValueObjects.Attribute) i] = _values[i];
            }

            return result;
        }

        public AttributeValue this[int stat]
        {
            get => _values[stat];
            set => _values[stat] = value;
        }

        public AttributeValue this[Combat.Local.Domain.ValueObjects.Attribute stat]
        {
            get => _values[(int) stat];
            set => _values[(int) stat] = value;
        }

        public static StatsTable operator +(StatsTable value1, StatsTable value2)
        {
            AttributeValue[] values = new AttributeValue[AttributesCount];

            for (int i = 0; i < AttributesCount; i++)
            {
                values[i] = value1[i] + value2[i];
            }

            return new StatsTable(values);
        }
    }
}
