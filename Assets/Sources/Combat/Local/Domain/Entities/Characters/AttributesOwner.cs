using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct AttributesOwner
    {
        public const int AttributeCount = (int)(Common.ValueObjects.Attribute.PARRY + 1);

        private readonly ReadOnlySpan<AttributeValue> _baseValues;
        private readonly ReadOnlySpan<float> _values;

        public AttributesOwner(EntityId id, ReadOnlySpan<AttributeValue> baseValues)
        {
            Id = id;

            _baseValues = baseValues;
            _values = default;
        }

        public AttributesOwner(EntityId id, ReadOnlySpan<AttributeValue> baseValues, ReadOnlySpan<float> bonuses)
        {
            Id = id;

            _baseValues = baseValues;
            _values = bonuses;
        }

        public EntityId Id { get; }

        public float this[Common.ValueObjects.Attribute attribute]
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

        public AttributeValue GetBaseValue(Common.ValueObjects.Attribute attribute) => _baseValues[(int)attribute];

        public float GetAttributeValue(Common.ValueObjects.Attribute attribute) => this[attribute];
        public float GetHasteModifier() => 1f + GetAttributeValue(Common.ValueObjects.Attribute.Haste) * 0.007f;
        public float GetVersalityModifier() => 1f + GetAttributeValue(Common.ValueObjects.Attribute.Versality) * 0.007f;
        public float GetMaxHealthBonus() => GetAttributeValue(Common.ValueObjects.Attribute.Endurance) * 10f;

        public ReadOnlySpan<AttributeValue> GetAllBase() => _baseValues;
        public ReadOnlySpan<float> GetAll() => _values;
    }
}
