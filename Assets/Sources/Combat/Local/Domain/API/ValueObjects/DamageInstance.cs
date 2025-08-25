using Combat.Local.Domain.API.DTO;

namespace Combat.Local.Domain.API.ValueObjects
{
    public ref struct DamageInstance
    {
        private const float DamagePercentConvertionRatio = 0.01f;

        public DamageInstance(Unit target, Unit attacker, ScriptedSkill source, float originalDamage, DamageFlags flags)
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
        public ScriptedSkill Source { get; }
        public float OriginalDamage { get; }

        public DamageFlags Flags { get; set; }
        public float BaseDamage { get; set; }
        public float DamagePercent { get; set; }

        public float GetCurrentDamage()
        {
            if (Target == null)
            {
                return 0;
            }

            float attackerVersalityBonus = Attacker == null ? 1 : Attacker.GetVersalityModifier();

            return System.Math.Max(0, BaseDamage * DamagePercent * DamagePercentConvertionRatio * attackerVersalityBonus / Target.GetVersalityModifier());
        }
    }
}
