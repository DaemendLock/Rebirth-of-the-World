using CastStateSkill;

using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct Skill
    {
        public Skill(SkillId id, bool canCast, bool startAction, bool allowMovement, IFrameData frameData)
        {
            Id = id;
            CanCast = canCast;
            StartAction = startAction;
            AllowMovement = allowMovement;
            FrameData = frameData;
        }

        public SkillId Id { get; }
        public bool CanCast { get; }
        public bool StartAction { get; }
        public bool AllowMovement { get; }
        public IFrameData FrameData { get; }
    }
}
