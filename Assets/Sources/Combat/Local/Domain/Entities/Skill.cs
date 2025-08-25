using CastStateSkill;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Flags;

namespace Combat.Local.Domain.Entities
{
    public struct Skill
    {
        public Skill(SkillId id, SkillFlags flags, IFrameData frameData)
        {
            Id = id;
            Flags = flags;
            FrameData = frameData;
        }

        public EntityId Owner => default;
        public SkillId Id { get; }
        public SkillFlags Flags { get; set; }
        public IFrameData FrameData { get; set; }

        public override int GetHashCode() => Id.Value;

        public override bool Equals(object obj) => obj is Skill other && Equals(other);

        public bool Equals(Skill other) => Id == other.Id;
    }
}
