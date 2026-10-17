using Combat.Common.ValueObjects;

namespace Combat.Local.Application.ValueObjects
{
    public readonly struct AttributeModifcation
    {
        public readonly float BaseValue;
        public readonly float Percent;
        public readonly float BonusValue;

        public AttributeModifcation(AttributeValue value)
        {
            BaseValue = value.BaseValue;
            Percent = value.Percent;
            BonusValue = value.Bonus;
        }

        public AttributeModifcation(float baseValue, float percent, float bonusValue)
        {
            BaseValue = baseValue;
            Percent = percent;
            BonusValue = bonusValue;
        }

        public static AttributeModifcation operator +(AttributeModifcation value1, AttributeModifcation value2)
        {
            return new(value1.BaseValue + value2.BaseValue, value1.Percent + value2.Percent, value1.BonusValue + value2.BonusValue);
        }
    }
}
