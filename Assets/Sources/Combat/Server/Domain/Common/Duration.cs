namespace Server.Combat.Domain.Common
{
    public interface IDuration
    {
        bool Expired { get; }
    }

    public readonly struct Duration : IDuration
    {
        public readonly static Duration Infinite = new(0, long.MaxValue);

        public Duration(float duration)
        {
            StartTime = Time.CurrentTime;
            EndTime = StartTime + (long) (duration * 1000);
        }

        public Duration(long duration)
        {
            StartTime = Time.CurrentTime;
            EndTime = StartTime + duration;
        }

        private Duration(long start, long end)
        {
            StartTime = start;
            EndTime = end;
        }

        public long FullTime => EndTime - StartTime;
        public long StartTime { get; }
        public long EndTime { get; }

        public long Left => EndTime - Time.CurrentTime;
        public bool Expired => Time.CurrentTime >= EndTime;

        public Duration Extended(long time) => new(StartTime, EndTime + time);

        public static Duration operator +(Duration duration, long value) => duration.Extended(value);

        public static Duration operator -(Duration duration, long value) => duration.Extended(-value);

        public static explicit operator Duration(long value) => new(value);

        public static explicit operator Duration(float value) => new(value);
    }
}
