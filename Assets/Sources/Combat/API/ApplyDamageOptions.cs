using Combat.API.DTO;
using Combat.Common.Flags;

namespace Combat.API.Utils
{
    public struct ApplyDamageOptions
    {
        public ApplyDamageOptions(Unit target, Unit attacker, AbilityApi skill, float healthDamage, float composureDamage, DamageFlags flags)
        {
            Target = target;
            Attacker = attacker;
            Source = skill;
            HealthDamage = healthDamage;
            ComposureDamage = composureDamage;
            Flags = flags;
        }

        public Unit Target { get; set; }
        public Unit Attacker { get; set; }
        public AbilityApi Source { get; set; }
        public float HealthDamage { get; set; }
        public float ComposureDamage { get; set; }
        public float OriginalDamage { readonly get => HealthDamage; set => HealthDamage = value; }
        public DamageFlags Flags { get; set; }

        public ApplyDamageInfo AsDamageData => new(Attacker, Source, HealthDamage, ComposureDamage, Flags);
    }

    public static class ApplyDamageOptionsExtension
    {
        public static void ApplyDamage(this ApplyDamageOptions damage)
        {
            damage.Target.ApplyDamage(damage.AsDamageData);
        }
    }
}
