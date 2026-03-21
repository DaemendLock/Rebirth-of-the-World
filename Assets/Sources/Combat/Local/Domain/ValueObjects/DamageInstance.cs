using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.ValueObjects
{
    public ref struct HealingInstance
    {
        public HealingInstance(EntityId target, float originalHealing, HealingFlags flags, EntityId? healer, EventSource source)
        {
            Target = target;
            Healer = healer;
            OriginalHealing = originalHealing;
            Source = source;
            Healing = originalHealing;
            Flags = flags;
        }

        public HealingInstance(EntityId target, float originalHealing, float healing, HealingFlags flags, EntityId? healer, EventSource source)
        {
            Target = target;
            Healer = healer;
            OriginalHealing = originalHealing;
            Source = source;
            Healing = healing;
            Flags = flags;
        }

        public EntityId Target { get; }
        public EntityId? Healer { get; }
        public float OriginalHealing { get; }
        public EventSource Source { get; }
        public float Healing { get; set; }
        public HealingFlags Flags { get; set; }
    }

    public ref struct DamageInstance
    {
        public EntityId Target { get; }
        public EntityId? Attacker { get; }
        public float OriginalDamage { get; }
        public EventSource Source { get; }
        public float Damage { get; set; }
        public DamageFlags Flags { get; set; }

        public DamageInstance(EntityId target, float originalDamage, float damage, DamageFlags flags, EntityId? attacker, EventSource source)
        {
            Target = target;
            Attacker = attacker;
            OriginalDamage = originalDamage;
            Source = source;
            Damage = damage;
            Flags = flags;
        }

        public DamageInstance(EntityId target, float originalDamage, DamageFlags flags, EntityId? attacker, EventSource source)
        {
            Target = target;
            OriginalDamage = originalDamage;
            Source = source;
            Damage = originalDamage;
            Flags = flags;
            Attacker = attacker;
        }
    }
}
