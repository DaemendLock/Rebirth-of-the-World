using Combat.Common.Flags;

namespace Combat.API.ValueObjects
{
    public readonly ref struct DamageInstanceApi
    {
        private const float DamagePercentConvertionRatio = 0.01f;

        public DamageInstanceApi(Unit target, Unit attacker, SkillApi source, float originalDamage, DamageFlags flags)
        {
            Attacker = attacker;
            Target = target;
            Source = source;
            OriginalDamage = originalDamage;
            Flags = flags;
            BaseDamage = originalDamage;
            DamagePercent = 100f;
        }

        public Unit Target { get; }
        public Unit Attacker { get; }
        public SkillApi Source { get; }
        public float OriginalDamage { get; }

        public DamageFlags Flags { get; }
        public float BaseDamage { get; }
        public float DamagePercent { get; }

        public float GetCurrentDamage()
        {
            if (Target == null)
            {
                return 0;
            }

            float attackerVersalityBonus = Attacker == null ? 1 : Attacker.GetVersalityModifier();

            return System.Math.Max(0, BaseDamage * DamagePercent * DamagePercentConvertionRatio * attackerVersalityBonus / Target.GetVersalityModifier());
        }

        //public DomainDamageInstance GetDamageInstance() => new(Target.Id, GetCurrentDamage(), Flags, new(Source.Caster.Id, Source.Id));
    }
}
