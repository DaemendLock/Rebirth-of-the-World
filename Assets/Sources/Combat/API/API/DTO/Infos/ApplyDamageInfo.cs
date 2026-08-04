using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyDamageInfo
    {
        public readonly float Damage;
        public readonly IAbilityApi Source;
        public readonly IUnit Attacker;
        public readonly DamageFlags Flags;

        public ApplyDamageInfo(IUnit attacker, IAbilityApi source, float damage, DamageFlags flags)
        {
            Damage = damage;
            Source = source;
            Attacker = attacker;
            Flags = flags;
        }
    }
}
