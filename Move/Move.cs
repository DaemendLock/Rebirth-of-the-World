﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Move
{
    public readonly struct HealthChangePrecessingData
    {
        public readonly Unit Appier;
        public readonly HealthChangeProcessingType Action;
        public readonly float Value;
    }

    public static class Time
    {
        public static float time => (float) CombatTime.Time / 1000;
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
}
