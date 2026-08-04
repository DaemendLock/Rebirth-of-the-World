using Combat.API.DTO;
using Combat.Common.Flags;

namespace Combat.API.Utils
{
    public struct ApplyDamageOptions
    {
        public ApplyDamageOptions(IUnit target, IUnit attacker, IAbilityApi skill, float originalDamage, DamageFlags flags)
        {
            Target = target;
            Attacker = attacker;
            Source = skill;
            OriginalDamage = originalDamage;
            Flags = flags;
        }

        public IUnit Target { get; set; }
        public IUnit Attacker { get; set; }
        public IAbilityApi Source { get; set; }
        public float OriginalDamage { get; set; }
        public DamageFlags Flags { get; set; }

        public ApplyDamageInfo AsDamageData => new(Attacker, Source, OriginalDamage, Flags);
    }

    public static class ApplyDamageOptionsExtension
    {
        public static void ApplyDamage(this ApplyDamageOptions damage)
        {
            damage.Target.ApplyDamage(damage.AsDamageData);
        }
    }
}
