using System.Linq;

namespace CastStateSkill
{
    public class FrameData : IFrameData
    {
        public const int FrameRate = 60;

        private readonly float[] _typeChangesTime;
        private readonly float _duration;

        public FrameData(float duration, float[] timeMarks)
        {
            _duration = duration;
            _typeChangesTime = timeMarks.OrderBy(value => value).Select(value => value * _duration).ToArray();
        }

        public int TotalFrames => (int) (_duration * FrameRate);
        public float FullDuration => _duration;

        public SkillCastState GetCastState(float time)
        {
            if (time > _duration)
            {
                return SkillCastState.Inactive;
            }

            if (_typeChangesTime.Length == 0 || time > _typeChangesTime[^1])
            {
                return SkillCastState.Recovery;
            }

            for (int i = _typeChangesTime.Length - 2; i >= 0; i--)
            {
                if (time <= _typeChangesTime[i])
                {
                    continue;
                }

                return (i & 1) == 0 ? SkillCastState.Active : SkillCastState.Gap;
            }

            return SkillCastState.Startup;
        }
    }
}
