using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct Attributes
    {
        public const int AttributeCount = (int)(Common.ValueObjects.Attribute.PARRY + 1);

        private readonly ReadOnlySpan<AttributeValue> _values;
        private readonly ReadOnlySpan<AttributeValue> _bonuses;

        public Attributes(EntityId id, ReadOnlySpan<AttributeValue> baseValues, Span<AttributeValue> bonuses)
        {
            Id = id;

            _values = baseValues;
            _bonuses = bonuses;
        }

        public EntityId Id { get; }

        public AttributeValue this[Common.ValueObjects.Attribute attribute] => (_bonuses == default) ? _values[(int)attribute] : (_values[(int)attribute] + _bonuses[(int)attribute]);

        public AttributeValue GetBaseValue(Common.ValueObjects.Attribute attribute) => _values[(int)attribute];

        public float GetAttributeValue(Common.ValueObjects.Attribute attribute) => this[attribute].CalculatedValue;
        public float GetHasteModifier() => 1f + GetAttributeValue(Common.ValueObjects.Attribute.Haste) * 0.007f;
        public float GetVersalityModifier() => 1f + GetAttributeValue(Common.ValueObjects.Attribute.Versality) * 0.007f;
        public float GetMaxHealthBonus() => GetAttributeValue(Common.ValueObjects.Attribute.Endurance) * 10f;

        public ReadOnlySpan<AttributeValue> GetAllBase() => _values;
    }
}
