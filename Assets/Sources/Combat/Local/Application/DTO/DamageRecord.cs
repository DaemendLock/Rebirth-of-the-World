using Combat.Common.Flags;
using Combat.Common.Primitives;

namespace Combat.Local.Application.DTO
{
    public readonly ref struct DamageRecord
    {
        public DamageRecord(UnitId target, float originalHealthDamage, float originalComposureDamage,
            float finalHealthDamage, float finalComposureDamage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            Target = target;
            OriginalHealthDamage = originalHealthDamage;
            OriginalComposureDamage = originalComposureDamage;
            FinalHealthDamage = finalHealthDamage;
            FinalComposureDamage = finalComposureDamage;
            Flags = flags;
            Attacker = attacker;
            Skill = source;
        }

        public UnitId Target { get; }
        public float OriginalHealthDamage { get; }
        public float OriginalComposureDamage { get; }
        public float FinalHealthDamage { get; }
        public float FinalComposureDamage { get; }
        public DamageFlags Flags { get; }

        public UnitId? Attacker { get; }
        public AbilityKey? Skill { get; }
    }
}
