using System;
using Utils.Interfaces;

namespace Utils
{

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