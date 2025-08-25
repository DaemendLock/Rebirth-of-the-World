using Combat.Local.Domain.API.DTO;

namespace Combat.Local.Domain.API
{
    public struct ApplyDamageOptions
    {
        public ApplyDamageOptions(Unit target, Unit attacker, ScriptedSkill skill, float originalDamage, DamageFlags flags)
        {
            Target = target;
            Attacker = attacker;
            Source = skill;
            OriginalDamage = originalDamage;
            Flags = flags;
        }

        public Unit Target { get; set; }
        public Unit Attacker { get; set; }
        public ScriptedSkill Source { get; set; }
        public float OriginalDamage { get; set; }
        public DamageFlags Flags { get; set; }

        public DamageData AsDamageData => new(Attacker, Source, OriginalDamage, Flags);
    }

    public static class ApplyDamageOptionsExtension
    {
        public static void ApplyDamage(this ApplyDamageOptions damage)
        {
            damage.Target.ApplyDamage(damage.AsDamageData);
        }
    }
}
