using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Combat.Local.Data.Models
{
    public readonly struct HealingInstanceData
    {
        public HealingInstanceData(EntityId target, float healing, HealingFlags flags, EntityId? healer, SkillId? skill, EntityId? caster)
        {
            Target = target;
            Healing = healing;
            Flags = flags;
            Healer = healer;
            Skill = skill;
            Caster = caster;
        }

        public EntityId Target { get; }
        public float Healing { get; }
        public HealingFlags Flags { get; }
        public EntityId? Healer { get; }
        public SkillId? Skill { get; }
        public EntityId? Caster { get; }
    }

    public readonly struct DamageInstanceData
    {
        public DamageInstanceData(EntityId target, float damage, DamageFlags flags, EntityId? attacker, SkillId? skill, EntityId? caster)
        {
            Target = target;
            Damage = damage;
            Flags = flags;
            Attacker = attacker;
            Skill = skill;
            Caster = caster;
        }

        public EntityId Target { get; }
        public float Damage { get; }
        public DamageFlags Flags { get; }
        public EntityId? Attacker { get; }
        public SkillId? Skill { get; }
        public EntityId? Caster { get; }
    }
}
