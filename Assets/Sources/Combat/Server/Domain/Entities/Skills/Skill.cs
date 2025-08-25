using CastStateSkill;

using Server.Combat.Domain.Skills;

namespace Server.Combat.Domain.Entities
{
    public class Skill : ISkill
    {
        public Skill(SkillId id, SkillFlags flags, float cooldown, IFrameData frameData)
        {
            Id = id;
            Flags = flags;
            Cooldown = cooldown;
            FrameData = frameData;
        }

        public SkillId Id { get; }
        public SkillFlags Flags { get; }
        public float Cooldown { get; }
        public IFrameData FrameData { get; }

        public sealed override int GetHashCode() => Id.Value;

        public sealed override bool Equals(object obj) => obj is ISkill other && Equals(other);

        public bool Equals(ISkill other) => Id == other.Id;
    }
}
