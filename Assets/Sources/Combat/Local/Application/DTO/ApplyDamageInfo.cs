using Combat.Common.Flags;
using Combat.Common.Primitives;

namespace Combat.Local.Application.DTO
{
    public readonly ref struct ApplyDamageInfo
    {
        public readonly float HealthDamage;
        public readonly float ComposureDamage;
        public readonly UnitId Target;
        public readonly DamageFlags Flags;
        public readonly UnitId? Attacker;
        public readonly AbilityKey? Source;
    }
}
