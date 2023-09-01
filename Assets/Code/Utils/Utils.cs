using System;

namespace Utils
{
    public interface Value<T>
    {
        T Evaluate();
    }

    [Serializable]
    public struct PercentModifiedValue
    {
        private const float PERCENT_CONVERTION_VALUE = 0.01f;

        public float BaseValue;
        public float Percent;

        public PercentModifiedValue(float baseValue, float percent)
        {
            BaseValue = baseValue;
            Percent = percent;
        }

        public float CalculatedValue => BaseValue * Percent * PERCENT_CONVERTION_VALUE;

        public static PercentModifiedValue operator +(PercentModifiedValue value1, PercentModifiedValue value2) => new PercentModifiedValue(value1.BaseValue + value2.BaseValue, value1.Percent + value2.Percent);
        public static PercentModifiedValue operator +(PercentModifiedValue value1, float value) => new PercentModifiedValue(value1.BaseValue + value, value1.Percent);

        public static PercentModifiedValue operator -(PercentModifiedValue value) => new PercentModifiedValue(-value.BaseValue, -value.Percent);
        public static PercentModifiedValue operator -(PercentModifiedValue value1, PercentModifiedValue value2) => new PercentModifiedValue(value1.BaseValue - value2.BaseValue, value1.Percent - value2.Percent);
    }

    public readonly struct DynamicValue<T> : Value<T>
    {
        private readonly Func<T> _func;

        public DynamicValue(Func<T> func)
        {
            _func = func;
        }

        public T Evaluate()
        {
            return _func.Invoke();
        }
    }

    public readonly struct ConstantValue<T> : Value<T>
    {
        private readonly T _value;
        public ConstantValue(T value)
        {
            _value = value;
        }

        public T Evaluate()
        {
            return _value;
        }
    }
}