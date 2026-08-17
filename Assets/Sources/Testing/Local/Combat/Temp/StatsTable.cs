using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System;

namespace Temp.Domain.Implementations
{
    [Serializable]
    public class StatsTable
    {
        public const int AttributesCount = (int)UnitAttribute.PARRY + 1;

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
        });

        private readonly AttributeValue[] _values;

        private StatsTable(AttributeValue[] values)
        {
            _values = new AttributeValue[AttributesCount];
            values.AsSpan().CopyTo(_values);
        }

        public void Add(StatsTable table)
        {
            for (int i = 0; i < AttributesCount; i++)
            {
                _values[i] += table._values[i];
            }
        }

        public AttributeValue this[int stat]
        {
            get => _values[stat];
            set => _values[stat] = value;
        }

        public AttributeValue this[UnitAttribute stat]
        {
            get => _values[(int)stat];
            set => _values[(int)stat] = value;
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

        public AttributeValue[] ToAttributeArray()
        {
            AttributeValue[] attributeValue = new AttributeValue[AttributesOwner.AttributeCount];

            for (int i = 0; i < attributeValue.Length; i++)
            {
                attributeValue[i] = this[(UnitAttribute)i];
            }

            return attributeValue;
        }
    }
}
