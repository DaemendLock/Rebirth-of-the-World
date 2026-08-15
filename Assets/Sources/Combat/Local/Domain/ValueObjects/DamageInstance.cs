using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.ValueObjects
{
    public struct HealthValue
    {
        public HealthValue(float current, float @default)
        {
            if (float.IsNaN(current))
            {
                throw new System.InvalidOperationException($"{nameof(current)}: Value can't be NaN");
            }

            if (float.IsNaN(@default))
            {
                throw new System.InvalidOperationException($"{nameof(@default)}: Value can't be NaN");
            }

            Default = @default;
            Current = current;
        }

        public float Default { get; set; }
        public float Current { get; set; }
    }

    public ref struct HealingInstance
    {
        public HealingInstance(UnitId target, float originalHealing, HealingFlags flags, UnitId? healer, AbilityKey? source)
        {
            Target = target;
            Healer = healer;
            OriginalHealing = originalHealing;
            Source = source;
            Healing = originalHealing;
            Flags = flags;
        }

        public HealingInstance(UnitId target, float originalHealing, float healing, HealingFlags flags, UnitId? healer, AbilityKey? source)
        {
            Target = target;
            Healer = healer;
            OriginalHealing = originalHealing;
            Source = source;
            Healing = healing;
            Flags = flags;
        }

        public UnitId Target { get; }
        public UnitId? Healer { get; }
        public float OriginalHealing { get; }
        public AbilityKey? Source { get; }
        public float Healing { get; set; }
        public HealingFlags Flags { get; set; }
    }

    public struct DamageInstance
    {
        public UnitId Target { get; }
        public UnitId? Attacker { get; }
        public float OriginalDamage { get; }
        public AbilityKey? Source { get; }
        public float Damage { get; set; }
        public DamageFlags Flags { get; set; }

        public DamageInstance(UnitId target, float originalDamage, float damage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            Target = target;
            Attacker = attacker;
            OriginalDamage = originalDamage;
            Source = source;
            Damage = damage;
            Flags = flags;
        }

        public DamageInstance(UnitId target, float originalDamage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
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
