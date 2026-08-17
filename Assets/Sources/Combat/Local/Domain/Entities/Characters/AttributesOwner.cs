using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly struct AttributesOwner : IUnitComponent
    {
        public const int AttributeCount = (int)(UnitAttribute.PARRY + 1);

        private readonly AttributeValue[] _baseValues;
        private readonly AttributeValue[] _values;

        public AttributesOwner(UnitId id, AttributeValue[] baseValues)
        {
            Id = id;

            _baseValues = baseValues;
            _values = new AttributeValue[baseValues.Length];
        }

        public UnitId Id { get; }

        public ReadOnlySpan<AttributeValue> BaseValues => _baseValues;

        public readonly float this[UnitAttribute attribute]
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

                return _values[index].CalculatedValue;
            }
        }

        public void Clear() => _baseValues.CopyTo(_values, 0);

        public void ApplyModifications(AttributesModification modification)
        {
            for (int i = 0; i < _baseValues.Length; i++)
            {
                AttributeModifier modifier = modification[(UnitAttribute)i];
                _values[i] = new(_baseValues[i].BaseValue + modifier.BaseValue, _baseValues[i].Percent + modifier.Percent, modifier.BonusValue);
            }
        }

        public readonly float GetAttributeValue(UnitAttribute attribute) => this[attribute];
        public readonly float GetHasteModifier() => 1f + GetAttributeValue(UnitAttribute.Haste) * 0.007f;
        public readonly float GetVersalityModifier() => 1f + GetAttributeValue(UnitAttribute.Versality) * 0.007f;
        public readonly float GetMaxHealthBonus() => GetAttributeValue(UnitAttribute.Endurance) * 10f;

        public readonly ReadOnlySpan<AttributeValue> GetAll() => _values;
    }
}
