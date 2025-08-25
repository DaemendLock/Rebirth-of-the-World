using System;

namespace Utils.DataTypes
{
    [Serializable]
    public struct PercentModifiedValue
    {
        private const float PercentConvertionMultiplier = 0.01f;

        public float BaseValue;
        public float Percent;

        public PercentModifiedValue(float baseValue, float percent)
        {
            BaseValue = baseValue;
            Percent = percent;
        }

        public float CalculatedValue => BaseValue * Percent * PercentConvertionMultiplier;

        public static PercentModifiedValue operator +(PercentModifiedValue value1, PercentModifiedValue value2) => new(value1.BaseValue + value2.BaseValue, value1.Percent + value2.Percent);
        public static PercentModifiedValue operator +(PercentModifiedValue value1, float value) => new(value1.BaseValue + value, value1.Percent);

        public static PercentModifiedValue operator -(PercentModifiedValue value) => new(-value.BaseValue, -value.Percent);
        public static PercentModifiedValue operator -(PercentModifiedValue value1, PercentModifiedValue value2) => new(value1.BaseValue - value2.BaseValue, value1.Percent - value2.Percent);

        public static PercentModifiedValue operator *(PercentModifiedValue value1, float value2) =>
            new(value1.BaseValue * value2, value1.Percent * value2);
    }
}
