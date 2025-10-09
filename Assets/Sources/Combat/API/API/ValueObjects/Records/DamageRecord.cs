using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct DamageRecord
    {
        public DamageRecord(Unit target, float originalDamage, float finalDamage, DamageFlags flags, Unit attacker, SkillApi source)
        {
            Attacker = attacker;
            Target = target;
            OriginalDamage = originalDamage;
            FinalDamage = finalDamage;
            Source = source;
            Flags = flags;
        }

        public Unit Target { get; }
        public Unit Attacker { get; }
        public float OriginalDamage { get; }
        public float FinalDamage { get; }
        public SkillApi Source { get; }
        public DamageFlags Flags { get; }
    }
}
