using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly ref struct HealingInstance
    {
        public HealingInstance(EntityId target, float originalHealing, float healing, HealingFlags flags, EntityId? healer, EventSource source)
        {
            Target = target;
            Healer = healer;
            OriginalHealing = originalHealing;
            Source = source;
            FinalHealing = healing;
            Flags = flags;
        }

        public EntityId Target { get; }
        public float OriginalHealing { get; }
        public float FinalHealing { get; }
        public HealingFlags Flags { get; }
        public EntityId? Healer { get; }
        public EventSource Source { get; }
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

        public DamageInstance(EntityId target, float originalDamage, EntityId? attacker, DamageFlags flags, EventSource source)
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
