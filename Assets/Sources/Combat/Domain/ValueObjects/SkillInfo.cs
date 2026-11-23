using Combat.Common.Flags;
using Combat.Common.Primitives;

using System.Collections.Generic;

namespace Combat.Domain.ValueObjects
{
    public readonly struct SkillInfo
    {
        public SkillInfo(SkillFlags flags, float baseCooldown, IReadOnlyCollection<ActionId> actions)
        {
            Flags = flags;
            BaseCooldown = baseCooldown;
            Actions = actions;
        }

        public SkillFlags Flags { get; }

        public float BaseCooldown { get; }

        public IReadOnlyCollection<ActionId> Actions { get; }
    }
}
