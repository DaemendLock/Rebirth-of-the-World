using Combat.Local.Domain.Entities;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct AttributesData
    {
        private readonly Attributes _baseValues;
        private readonly AttributeValue[] _values;

        public AttributesData(Attributes baseValues, AttributeValue[] buffer)
        {
            _values = buffer;
            _baseValues = baseValues;
        }

        public AttributeValue GetBaseValue(Attribute attribute) => _baseValues[attribute];

        public AttributeValue this[Attribute attribute]
        {
            get => _baseValues.GetBaseValue(attribute) + _values[(int) attribute];
            set => _values[(int) attribute] = value - _baseValues.GetBaseValue(attribute);
        }
    }
}
