using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.DTO
{
    public readonly ref struct DamageData
    {
        public readonly float Damage;
        public readonly Skill Source;
        public readonly Unit Attacker;
        public readonly DamageFlags Flags;

        public DamageData(Unit attacker, Skill source, float damage, DamageFlags flags)
        {
            Damage = damage;
            Source = source;
            Attacker = attacker;
            Flags = flags;
        }
    }
}
