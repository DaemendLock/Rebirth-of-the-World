using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.Units.ValueObjects
{
    public class DamageInstance
    {
        public DamageInstance(Unit attacker, Unit victim, float damage, DamageFlags flags)
        {
            Attacker = attacker;
            Victim = victim;
            Damage = damage;
            Flags = flags;
        }

        public Unit Attacker { get; }
        public Unit Victim { get; }
        public float Damage { get; }
        public DamageFlags Flags { get; }
    }
}
