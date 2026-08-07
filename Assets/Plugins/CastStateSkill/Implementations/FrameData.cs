using System.Linq;

namespace CastStateSkill
{
    public class FrameData : IFrameData
    {
        public const int FrameRate = 60;

        private readonly float[] _typeChangesTimeNormalized;
        private readonly float _duration;

        public FrameData(float duration, float[] timeMarks)
        {
            _duration = duration;
            _typeChangesTimeNormalized = timeMarks.OrderBy(value => value).ToArray();
        }

        public int TotalFrames => (int)(_duration * FrameRate);

        public float FullDuration => _duration;

        public SkillCastState GetCastState(float time)
        {
            if (_duration <= 0 || time > _duration)
            {
                return SkillCastState.Inactive;
            }

            time /= _duration;

            if (_typeChangesTimeNormalized.Length == 0 || time > _typeChangesTimeNormalized[^1])
            {
                return SkillCastState.Recovery;
            }

            for (int i = _typeChangesTimeNormalized.Length - 2; i >= 0; i--)
            {
                if (time <= _typeChangesTimeNormalized[i])
                {
                    continue;
                }

                return (i & 1) == 0 ? SkillCastState.Active : SkillCastState.Gap;
            }

            return SkillCastState.Startup;
        }

        public float RecoveryEnterTimeNormalized => _typeChangesTimeNormalized.Length == 0
            ? 0
            : _typeChangesTimeNormalized[^1];

        public float RecoveryEnterTime => RecoveryEnterTimeNormalized * _duration;
    }
}
