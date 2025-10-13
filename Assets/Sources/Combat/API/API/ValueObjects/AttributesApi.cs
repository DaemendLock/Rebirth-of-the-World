using Combat.Common.ValueObjects;

using System;

namespace Combat.API.ValueObjects
{
    public ref struct AttributesApi
    {
        private readonly ReadOnlySpan<AttributeValue> _baseValues;
        private readonly Span<AttributeValue> _bonusValues;

        public AttributesApi(ReadOnlySpan<AttributeValue> baseValues, Span<AttributeValue> bonusValues) : this()
        {
            _baseValues = baseValues;
            _bonusValues = bonusValues;
        }

        public AttributeValue Attack { get; set; }
        public AttributeValue Spellpower { get; set; }

        public AttributeValue Haste { get; set; }
        public float Lethality { get; set; }
        public float Versality { get; set; }

        public AttributeValue Movespeed { get; set; }
        public AttributeValue Endurance { get; set; }
    }
}
