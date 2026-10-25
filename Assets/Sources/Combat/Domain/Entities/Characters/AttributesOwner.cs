using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.ValueObjects;

using System;

namespace Combat.Domain.Entities
{
    public readonly struct AttributesOwner
    {
        public const int AttributeCount = (int)(UnitAttribute.Endurance + 1);

        private readonly AttributeValue[] _baseValues;
        private readonly float?[] _cachedValues;

        public AttributesOwner(UnitId id, ReadOnlySpan<AttributeValue> baseValues)
        {
            Id = id;

            _baseValues = baseValues.ToArray();
            _cachedValues = new float?[_baseValues.Length];
        }

        public UnitId Id { get; }

        public ReadOnlySpan<AttributeValue> BaseValues => _baseValues;

        public readonly float this[UnitAttribute attribute]
        {
            get
            {
                int index = (int)attribute;

                if (index < 0 || index > _baseValues.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(attribute));
                }

                if (_cachedValues[index].HasValue == false)
                {
                    return _baseValues[index].CalculatedValue;
                }

                return _cachedValues[index].Value;
            }
        }

        public void Clear() => Array.Clear(_cachedValues, 0, _cachedValues.Length);

        public void SetModification(UnitAttribute attribute, AttributeModifcation modifcation)
        {
            int index = (int)attribute;

            if (index < 0 || index > _baseValues.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(attribute));
            }

            AttributeValue baseValue = _baseValues[index];
            AttributeValue value = new(baseValue.BaseValue + modifcation.BaseValue, baseValue.Percent + modifcation.Percent, baseValue.Bonus + modifcation.BonusValue);
            _cachedValues[index] = value.CalculatedValue;
        }

        public readonly AttributeValue GetBaseValue(UnitAttribute attribute) => _baseValues[(int)attribute];

        public readonly float GetAttributeValue(UnitAttribute attribute) => this[attribute];

        public readonly float GetSwiftnessMovespeedBonus() => GetAttributeValue(UnitAttribute.Swiftness);

        public readonly float GetHasteModifier() => 1f + GetAttributeValue(UnitAttribute.Haste) * 0.007f;
        public readonly float GetVersalityModifier() => 1f + GetAttributeValue(UnitAttribute.Versality) * 0.007f;
        public readonly float GetMaxHealthBonus() => GetAttributeValue(UnitAttribute.Endurance) * 10f;
        public readonly float GetMaxComposureBonus() => GetAttributeValue(UnitAttribute.Endurance) * 0.3f;
    }
}
