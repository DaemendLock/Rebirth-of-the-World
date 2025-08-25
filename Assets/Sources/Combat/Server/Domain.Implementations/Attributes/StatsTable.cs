using System;

using Server.Combat.Domain.Attributes;

namespace Server.Combat.Domain.Implementations.Attributes
{
    [Serializable]
    public class StatsTable : IAttributeCollection<Domain.Attributes.Attribute>
    {
        public const int AttributesCount = (int) Domain.Attributes.Attribute.PARRY + 1;

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

        public StatsTable(IAttributeCollection<Domain.Attributes.Attribute> attributeCollection) : this()
        {
            Add(attributeCollection);
        }

        private StatsTable(AttributeValue[] values)
        {
            _values = new AttributeValue[AttributesCount];
            values.AsSpan().CopyTo(_values);
        }

        public Memory<AttributeValue> Memory => _values.AsMemory();

        public void Add(IAttributeCollection<Domain.Attributes.Attribute> table)
        {
            for (int i = 0; i < AttributesCount; i++)
            {
                _values[i] += table[(Domain.Attributes.Attribute) i];
            }
        }

        public void Add(StatsTable table)
        {
            for (int i = 0; i < AttributesCount; i++)
            {
                _values[i] += table._values[i];
            }
        }

        public void Populate(Memory<AttributeValue> values)
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

        public AttributeValue this[int stat]
        {
            get => _values[stat];
            set => _values[stat] = value;
        }

        public AttributeValue this[Domain.Attributes.Attribute stat]
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
