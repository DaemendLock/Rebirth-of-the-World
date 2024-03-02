namespace Server.Combat.Core.Utils
{
    public readonly struct Duration
    {
        public Duration(float duration)
        {
            StartTime = Time.time;
            EndTime = StartTime + (long) (duration * 1000);
        }

        public Duration(long duration)
        {
            StartTime = Time.time;
            EndTime = StartTime + duration;
        }

        private Duration(long start, long end)
        {
            StartTime = start;
            EndTime = end;
        }

        public float FullTime => (EndTime - StartTime) / 1000;
        public long StartTime { get; }
        public long EndTime { get; }

        public float Left => (EndTime - Time.time) / 1000;
        public bool Expired => Time.time >= EndTime;

        public Duration Extend(long time) => new Duration(StartTime, EndTime + time);

        public static Duration operator +(Duration duration, long value) => duration.Extend(value);

        public static Duration operator -(Duration duration, long value) => duration.Extend(-value);

        public static explicit operator Duration(long value) => new(value);

        public static explicit operator Duration(float value) => new(value);
    }
}
