using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public ref struct Ability
    {
        public Ability(SkillId skillId, SkillFlags flags, UnitId? owner, float cooldown, IReadOnlyCollection<ActionId> actions, IAbilityPropertyContainer properties)
        {
            SkillId = skillId;
            Flags = flags;
            Owner = owner;
            Actions = actions;
            Properties = properties;
            ActiveCooldown = cooldown;
        }

        public SkillId SkillId { get; }

        public SkillFlags Flags { get; }

        public UnitId? Owner { get; }

        public IAbilityPropertyContainer Properties { get; }

        public IReadOnlyCollection<ActionId> Actions { get; }

        public float ActiveCooldown { get; private set; }

        public readonly bool AllowMoment => Flags.HasFlag(SkillFlags.DontRestrictMovement);

        public void StartCooldown(float value)
        {
            ActiveCooldown = value;
        }

        public void ProgressCooldown(float time)
        {
            if (ActiveCooldown > time)
            {
                ActiveCooldown -= time;
            }
            else
            {
                ActiveCooldown = 0;
            }
        }
    }
}
