using System;

namespace Core.Combat.Utils
{
    public static class CombatTime
    {
        private static int _startTime;

        public static int Time => Environment.TickCount - _startTime;

        public static void SetStartTime(int startTime)
        {
            _startTime = startTime;
        }

        public static void Reset()
        {
            _startTime = 0;
        }
    }
}
