using System;

namespace Combat.Common.ValueObjects
{
    [Serializable]
    public struct AttributeValue
    {
        private const float PercentConvertionMultiplier = 0.01f;

        public AttributeValue(float baseValue = 0, float percent = 0, float bonus = 0)
        {
            BaseValue = baseValue;
            Percent = percent;
            Bonus = bonus;
        }

        public float CalculatedValue => BaseValue * Percent * PercentConvertionMultiplier + Bonus;

        public float BaseValue { get; set; }
        public float Percent { get; set; }
        public float Bonus { get; set; }

        public static AttributeValue operator +(AttributeValue value1, AttributeValue value2) => new(value1.BaseValue + value2.BaseValue, value1.Percent + value2.Percent, value1.Bonus + value2.Bonus);
        public static AttributeValue operator -(AttributeValue value1, AttributeValue value2) => new(value1.BaseValue - value2.BaseValue, value1.Percent - value2.Percent, value1.Bonus - value2.Bonus);
    }
}
