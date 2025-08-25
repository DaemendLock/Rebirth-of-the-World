using System;

namespace Server.Combat.Domain.Skills
{
    public interface ISkill : IEquatable<ISkill>
    {
        SkillId Id { get; }
        SkillFlags Flags { get; }
        float Cooldown { get; }
    }
}
