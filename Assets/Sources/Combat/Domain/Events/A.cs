using Combat.Common.Primitives;

namespace Combat.Domain.Events
{
    public readonly struct A
    {
    }

    public readonly struct UnitDamagedEvent
    {
        public readonly UnitId Victim;
        public readonly float OriginalHealthDamage;
        public readonly float FinalHealthDamage;
        public readonly float OriginalComposureDamage;
        public readonly float FinalComposureDamage;

        public readonly UnitId? Attacker;
        public readonly AbilityKey? Source;

        public UnitDamagedEvent(UnitId victim, float originalHealthDamage, float finalHealthDamage, float originalComposureDamage, float finalComposureDamage,
            UnitId? attacker = null, AbilityKey? source = null)
        {
            Victim = victim;
            OriginalHealthDamage = originalHealthDamage;
            FinalHealthDamage = finalHealthDamage;
            OriginalComposureDamage = originalComposureDamage;
            FinalComposureDamage = finalComposureDamage;
            Attacker = attacker;
            Source = source;
        }
    }
}

namespace Combat.Domain.Events
{
}
