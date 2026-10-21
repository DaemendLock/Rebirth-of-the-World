using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct DamageRecord
    {
        public DamageRecord(Unit target, float originalHealthDamage, float originalComposureDamage,
            float finalHealthDamage, float finalComposureDamage, DamageFlags flags, Unit attacker, AbilityApi source)
        {
            Attacker = attacker;
            Target = target;
            OriginalHealthDamage = originalHealthDamage;
            OriginalComposureDamage = originalComposureDamage;
            FinalHealthDamage = finalHealthDamage;
            FinalComposureDamage = finalComposureDamage;
            Source = source;
            Flags = flags;
        }

        public Unit Target { get; }
        public Unit Attacker { get; }
        public float OriginalHealthDamage { get; }
        public float OriginalComposureDamage { get; }
        public float FinalHealthDamage { get; }
        public float FinalComposureDamage { get; }
        public float OriginalDamage => OriginalHealthDamage;
        public float FinalDamage => FinalHealthDamage;
        public AbilityApi Source { get; }
        public DamageFlags Flags { get; }
    }
}
