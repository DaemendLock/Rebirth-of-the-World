using Combat.Common.ValueObjects;

namespace Combat.API.DTO
{
    public readonly ref struct AttributesData
    {
        private readonly System.ReadOnlySpan<AttributeValue> _baseValues;
        private readonly System.Span<AttributeValue> _bonusValues;

        public AttributesData(System.ReadOnlySpan<AttributeValue> baseValues, System.Span<AttributeValue> bonusValues)
        {
            _baseValues = baseValues;
            _bonusValues = bonusValues;
        }

        public AttributeValue GetBaseValue(Attribute attribute) => _baseValues[(int)attribute];

        public AttributeValue this[Attribute attribute]
        {
            get => _bonusValues[(int)attribute];
            set => _bonusValues[(int)attribute] = value;
        }
    }
}
