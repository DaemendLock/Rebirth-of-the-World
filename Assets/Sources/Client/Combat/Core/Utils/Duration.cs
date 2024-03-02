using UnityEngine;

namespace Client.Combat.Core.Utils
{
    public readonly struct Duration
    {
        private readonly float _startTime;
        private readonly float _endTime;

        public Duration(float duration)
        {
            if (duration < 0)
                duration = 0;

            _startTime = Time.time;
            _endTime = _startTime + duration;
        }

        public float Left => _endTime - _startTime;

        public bool Expired => _endTime <= _startTime;

        public float FullTime => _endTime - _startTime;
    }
}
