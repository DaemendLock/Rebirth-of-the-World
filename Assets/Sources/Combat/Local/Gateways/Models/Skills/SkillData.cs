using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Models
{
    public readonly struct SkillData
    {
        public readonly ISkillStrategy Strategy;
        public readonly SkillFlags Flags;
        public readonly IReadOnlyCollection<ActionId> Actions;

        public SkillData(SkillFlags flags, IReadOnlyCollection<ActionId> actions, ISkillStrategy strategy)
        {
            Strategy = strategy;
            Flags = flags;
            Actions = actions;
        }
    }
}
