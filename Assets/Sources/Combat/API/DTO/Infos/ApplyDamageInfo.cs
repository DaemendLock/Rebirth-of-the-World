using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyDamageInfo
    {
        public readonly float HealthDamage;
        public readonly float ComposureDamage;
        public readonly AbilityApi Source;
        public readonly Unit Attacker;
        public readonly DamageFlags Flags;

        public ApplyDamageInfo(Unit attacker, AbilityApi source, float damage, DamageFlags flags)
            : this(attacker, source, damage, 0f, flags)
        { }

        public ApplyDamageInfo(Unit attacker, AbilityApi source, float healthDamage, float composureDamage, DamageFlags flags)
        {
            HealthDamage = healthDamage;
            ComposureDamage = composureDamage;
            Source = source;
            Attacker = attacker;
            Flags = flags;
        }

        // Existing skill scripts that only supply damage continue to deal health damage.
        public float Damage => HealthDamage;
    }
}
