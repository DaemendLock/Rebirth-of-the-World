using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct Ability
    {
        public Ability(SkillId skillId, SkillFlags flags, UnitId? owner, IReadOnlyCollection<ActionId> actions)
        {
            SkillId = skillId;
            Flags = flags;
            Owner = owner;
            Actions = actions;
        }

        public SkillId SkillId { get; }

        public SkillFlags Flags { get; }

        public UnitId? Owner { get; }

        public IReadOnlyCollection<ActionId> Actions { get; }

        public readonly bool AllowMoment => Flags.HasFlag(SkillFlags.DontRestrictMovement);
    }
}
