using System;
using UnityEngine;

namespace Remaster.Utils
{
    public interface Value<T>
    {
        T Evaluate();
    }

    public enum CommandResult : byte
    {
        SUCCES,
        ON_COOLDOWN,
        INVALID_TARGET,
        NOT_IN_LOS,
        NOT_ENOUGHT_RESOURCE
    }

    public struct Position
    {
        public Vector3 Location;
        public Vector3 ViewDirection;
    }

    public readonly struct Duration
    {
        public Duration(float duration)
        {
            StartTime = Time.time;
            EndTime = StartTime + duration;
        }

        private Duration(float start, float end)
        {
            StartTime = start;
            EndTime = end;
        }

        public float FullTime => EndTime - StartTime;
        public float StartTime { get; }
        public float EndTime { get; }

        public float Left => EndTime - Time.time;
        public bool Expired => Time.time >= EndTime;
        public float ExistTime => Time.time - StartTime;

        public Duration Extend(float time)
        {
            return new Duration(StartTime, EndTime + time);
        }

        public static Duration operator +(Duration duration, float value)
        {
            return duration.Extend(value);
        }

        public static Duration operator -(Duration duration, float value)
        {
            return duration.Extend(-value);
        }
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

    public enum HealthChangeProcessingType
    {
        INITIAL_VALUE,
        CONSTANT_MODIFICATION,
        PERCENT_MODIFICATION,
        EVADE,
        PARRY,
        BLOCK,
        ABSORB,

    }

    public enum HealthChangeFailType
    {
        SUCCESS,
        EVADE,
        PARRY,
        BLOCK,
        ABSORB,
    }

    public readonly struct HealthChangePrecessingData
    {
        public readonly Unit Appier;
        public readonly HealthChangeProcessingType Action;
        public readonly float Value;
    }

    public class RotW
    {

    }

    public static class Time
    {
        /// <summary>
        /// The time at the beggining of unity frame
        /// </summary>
        public static float time => UnityEngine.Time.time;
    }
}