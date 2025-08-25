using UnityEngine;

namespace Client.Combat.Domain.Utils
{
    public readonly struct FloatDuration
    {
        private readonly float _startTime;
        private readonly float _endTime;

        public FloatDuration(float duration)
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
