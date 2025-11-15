using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using System.Collections.Generic;
using System.Linq;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct Skill
    {
        public Skill(SkillId id, bool canCast, IEnumerable<ActionId> associatedActions, SkillFlags flags)
        {
            Id = id;
            CanCast = canCast;
            Flags = flags;
            AssociatedActions = associatedActions.ToArray();
        }

        public SkillId Id { get; }
        public bool CanCast { get; }
        public SkillFlags Flags { get; }
        public IReadOnlyCollection<ActionId> AssociatedActions { get; }

        public bool AllowMoment => Flags.HasFlag(SkillFlags.DontRestrictMovement);
    }
}
