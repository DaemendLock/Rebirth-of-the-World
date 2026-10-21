using Combat.Common.Flags;
using Combat.Common.Primitives;

namespace Combat.Local.Application.DTO
{
    public readonly ref struct ApplyDamageInfo
    {
        public ApplyDamageInfo(UnitId target, float healthDamage, float composureDamage,
            DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            Target = target;
            HealthDamage = healthDamage;
            ComposureDamage = composureDamage;
            Flags = flags;
            Attacker = attacker;
            Source = source;
        }

        public readonly float HealthDamage;
        public readonly float ComposureDamage;
        public readonly UnitId Target;
        public readonly DamageFlags Flags;
        public readonly UnitId? Attacker;
        public readonly AbilityKey? Source;
    }
}
