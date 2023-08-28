using System;

namespace Remaster.Engine
{
    public static class CombatTime
    {
        private static long _startTime;

        public static long Time => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _startTime;

        public static void SetStartTime(long startTime)
        {
            _startTime = startTime;
        }

        public static void Reset()
        {
            _startTime = 0;
        }
    }
}
