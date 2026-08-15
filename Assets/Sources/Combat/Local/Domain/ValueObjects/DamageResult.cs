using Combat.Common.Flags;
using Combat.Common.Primitives;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly ref struct DamageResult
    {
        public DamageResult(UnitId target, float originalDamage, float finalDamage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            Target = target;
            OriginalDamage = originalDamage;
            FinalDamage = finalDamage;
            Flags = flags;
            Attacker = attacker;
            Skill = source;
        }

        public UnitId Target { get; }
        public float OriginalDamage { get; }
        public float FinalDamage { get; }
        public DamageFlags Flags { get; }

        public UnitId? Attacker { get; }
        public AbilityKey? Skill { get; }
    }
}
