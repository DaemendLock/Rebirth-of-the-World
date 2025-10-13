using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyDamageInfo
    {
        public readonly float Damage;
        public readonly SkillApi Source;
        public readonly Unit Attacker;
        public readonly DamageFlags Flags;

        public ApplyDamageInfo(Unit attacker, SkillApi source, float damage, DamageFlags flags)
        {
            Damage = damage;
            Source = source;
            Attacker = attacker;
            Flags = flags;
        }
    }
}
