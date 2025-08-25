using System;

namespace Server.Combat.Domain.Attributes
{
    [Serializable]
    public struct AttributeValue
    {
        private const float PercentConvertionMultiplier = 0.01f;

        public float BaseValue;
        public float Percent;

        public AttributeValue(float baseValue, float percent)
        {
            BaseValue = baseValue;
            Percent = percent;
        }

        public float CalculatedValue => BaseValue * Percent * PercentConvertionMultiplier;

        public static AttributeValue operator +(AttributeValue value1, AttributeValue value2) => new(value1.BaseValue + value2.BaseValue, value1.Percent + value2.Percent);
        public static AttributeValue operator +(AttributeValue value1, float value) => new(value1.BaseValue + value, value1.Percent);

        public static AttributeValue operator -(AttributeValue value) => new(-value.BaseValue, -value.Percent);
        public static AttributeValue operator -(AttributeValue value1, AttributeValue value2) => new(value1.BaseValue - value2.BaseValue, value1.Percent - value2.Percent);

        public static AttributeValue operator *(AttributeValue value1, float value2) =>
            new(value1.BaseValue * value2, value1.Percent * value2);
    }
}
