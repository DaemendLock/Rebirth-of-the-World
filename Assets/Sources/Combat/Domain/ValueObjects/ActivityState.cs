using CastStateSkill;

using Combat.Common.Primitives;

namespace Combat.Domain.ValueObjects
{
    public enum ActivityType : byte
    {
        Idle,
        Casting,

    }

    public readonly struct ActivityState
    {
        private ActivityState(ActivityType type, float activeTime)
        {
            Type = type;
            ActiveTime = activeTime;
        }

        public ActivityType Type { get; }

        public float ActiveTime { get; }

        public static ActivityState Idle => new(ActivityType.Idle, 0);

        public static ActivityState Casting() => new(ActivityType.Casting, 0);
    }

    public struct CastActivity
    {
        public CastActivity(SkillId skill, IFrameData frameData)
        {
            Skill = skill;
            FrameData = frameData;
            EffectiveTime = 0;
            IsReleased = false;
        }

        public readonly IFrameData FrameData { get; }

        public readonly SkillId Skill { get; }

        public readonly SkillCastState FrameType => FrameData.GetCastState(EffectiveTime);

        public float EffectiveTime { get; private set; }

        public bool IsReleased { get; private set; }

        public readonly bool CanChainInto(SkillId skillId) => false;

        public void Progress(float deltaTime)
        {
            EffectiveTime += deltaTime;

            if (IsReleased == false && EffectiveTime > FrameData.LastActiveExitTime)
            {
                EffectiveTime = FrameData.LastActiveExitTime;
            }
        }

        public void Release()
        {
            IsReleased = true;
        }
    }
}
