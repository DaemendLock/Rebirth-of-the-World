using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Models
{
    public readonly struct AbilityData
    {
        public readonly IAbilityPropertyContainer Properties;
        public readonly SkillFlags Flags;
        public readonly IReadOnlyCollection<ActionId> Actions;
        public readonly float ActiveCooldown;

        public AbilityData(Ability skill)
        {
            Properties = skill.Properties;
            Actions = skill.Actions;
            Flags = skill.Flags;
            ActiveCooldown = skill.ActiveCooldown;
        }

        public Ability ToAbility(UnitId? owner, SkillId id)
        {
            return new(id, Flags, owner, ActiveCooldown, Actions, Properties);
        }
    }
}
