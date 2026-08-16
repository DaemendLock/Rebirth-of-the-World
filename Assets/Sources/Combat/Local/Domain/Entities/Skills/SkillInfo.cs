using Combat.Common.Flags;
using Combat.Common.Primitives;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public readonly struct SkillInfo
    {
        public SkillInfo(SkillId skillId, SkillFlags flags, float baseCooldown, IReadOnlyCollection<ActionId> actions)
        {
            SkillId = skillId;
            Flags = flags;
            BaseCooldown = baseCooldown;
            Actions = actions;
        }

        public SkillId SkillId { get; }

        public SkillFlags Flags { get; }

        public float BaseCooldown { get; }

        public IReadOnlyCollection<ActionId> Actions { get; }
    }
}
