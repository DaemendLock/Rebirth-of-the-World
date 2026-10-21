using Combat.Common.Flags;

namespace Combat.API.ValueObjects
{
    public readonly ref struct DamageInstanceApi
    {
        private const float DamagePercentConvertionRatio = 0.01f;

        public DamageInstanceApi(Unit target, Unit attacker, AbilityApi source, float originalDamage, DamageFlags flags)
            : this(target, attacker, source, originalDamage, 0f, flags)
        { }

        public DamageInstanceApi(Unit target, Unit attacker, AbilityApi source,
            float originalHealthDamage, float originalComposureDamage, DamageFlags flags)
        {
            Attacker = attacker;
            Target = target;
            Source = source;
            OriginalHealthDamage = originalHealthDamage;
            OriginalComposureDamage = originalComposureDamage;
            Flags = flags;
            BaseHealthDamage = originalHealthDamage;
            BaseComposureDamage = originalComposureDamage;
            DamagePercent = 100f;
        }

        public Unit Target { get; }
        public Unit Attacker { get; }
        public AbilityApi Source { get; }
        public float OriginalHealthDamage { get; }
        public float OriginalComposureDamage { get; }
        public float OriginalDamage => OriginalHealthDamage;

        public DamageFlags Flags { get; }
        public float BaseHealthDamage { get; }
        public float BaseComposureDamage { get; }
        public float BaseDamage => BaseHealthDamage;
        public float DamagePercent { get; }

        public float GetCurrentDamage() => GetCurrentHealthDamage();

        public float GetCurrentHealthDamage() => GetCurrentDamage(BaseHealthDamage);

        public float GetCurrentComposureDamage() => GetCurrentDamage(BaseComposureDamage);

        private float GetCurrentDamage(float baseDamage)
        {
            if (Target is null)
            {
                return 0;
            }

            float attackerVersalityBonus = Attacker is null ? 1 : Attacker.GetVersalityModifier();

            return System.Math.Max(0, baseDamage * DamagePercent * DamagePercentConvertionRatio * attackerVersalityBonus / Target.GetVersalityModifier());
        }

    }
}
