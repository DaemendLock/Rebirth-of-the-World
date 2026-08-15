using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct AttributesOwner
    {
        public const int AttributeCount = (int)(Common.ValueObjects.UnitAttribute.PARRY + 1);

        private readonly ReadOnlySpan<AttributeValue> _baseValues;
        private readonly ReadOnlySpan<float> _values;

        public AttributesOwner(UnitId id, ReadOnlySpan<AttributeValue> baseValues) : this(id, baseValues, Span<float>.Empty)
        { }

        public AttributesOwner(UnitId id, ReadOnlySpan<AttributeValue> baseValues, ReadOnlySpan<float> finalValues)
        {
            Id = id;

            _baseValues = baseValues;
            _values = finalValues;
        }

        public UnitId Id { get; }

        public readonly float this[Common.ValueObjects.UnitAttribute attribute]
        {
            get
            {
                int index = (int)attribute;

                if (index < 0 || index > _values.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(attribute));
                }

                if (_values == default)
                {
                    return _baseValues[index].CalculatedValue;
                }

                return _values[index];
            }
        }

        public readonly AttributeValue GetBaseValue(Common.ValueObjects.UnitAttribute attribute) => _baseValues[(int)attribute];

        public readonly float GetAttributeValue(Common.ValueObjects.UnitAttribute attribute) => this[attribute];
        public readonly float GetHasteModifier() => 1f + GetAttributeValue(Common.ValueObjects.UnitAttribute.Haste) * 0.007f;
        public readonly float GetVersalityModifier() => 1f + GetAttributeValue(Common.ValueObjects.UnitAttribute.Versality) * 0.007f;
        public readonly float GetMaxHealthBonus() => GetAttributeValue(Common.ValueObjects.UnitAttribute.Endurance) * 10f;

        public readonly ReadOnlySpan<AttributeValue> GetAllBase() => _baseValues;
        public readonly ReadOnlySpan<float> GetAll() => _values;
    }
}
