using Combat.Common.Flags;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Gateways.Models
{
    public readonly struct SkillData
    {
        public readonly ISkillStrategy Strategy;
        public readonly SkillFlags Flags;

        public SkillData(ISkillStrategy strategy, SkillFlags flags)
        {
            Strategy = strategy;
            Flags = flags;
        }
    }
}
