using System;

namespace Utils.DataTypes
{
    public struct Resource
    {
        private float _value;
        private float _minValue;
        private float _maxValue;

        public Resource(float maxValue)
        {
            _maxValue = maxValue;
            _minValue = 0;
            _value = maxValue;
        }

        public Resource(float minValue, float maxValue)
        {
            _maxValue = maxValue;
            _minValue = minValue;
            _value = maxValue;
        }

        public float Value
        {
            get => _value;
            set => _value = Math.Clamp(value, MinValue, MaxValue);
        }
        public float MinValue { get => _minValue; set => Math.Max(0, value); }
        public float MaxValue { get => _maxValue; set => Math.Max(MinValue, value); }

        public static Resource operator +(Resource resource, float value)
        {
            Resource result = resource;

            result.Value += value;

            if (result.Value < resource.MinValue)
            {
                result.Value = resource.MinValue;
            }
            else if (result.Value > result.MaxValue)
            {
                result.Value = result.MaxValue;
            }

            return result;
        }

        public static Resource operator -(Resource resource, float value)
        {
            Resource result = resource;
            result.Value -= value;

            if (result.Value < result.MinValue)
            {
                result.Value = result.MinValue;
            }
            else if (result.Value > result.MaxValue)
            {
                result.Value = result.MaxValue;
            }

            return result;
        }

        public static bool operator >(Resource resource, float value)
        {
            return resource.Value > value;
        }

        public static bool operator >=(Resource resource, float value)
        {
            return resource.Value >= value;
        }

        public static bool operator <(Resource resource, float value)
        {
            return resource.Value < value;
        }

        public static bool operator <=(Resource resource, float value)
        {
            return resource.Value <= value;
        }

        public static bool operator ==(Resource resource, float value)
        {
            return Math.Abs(resource.Value - value) < 0.001;
        }

        public static bool operator !=(Resource resource, float value)
        {
            return Math.Abs(resource.Value - value) > 0.001;
        }

        public override string ToString()
        {
            return $"Resource:({_value}/{_minValue}-{_maxValue})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Resource resource &&
                   _value == resource._value &&
                   _minValue == resource._minValue &&
                   _maxValue == resource._maxValue;
        }
    }
}
