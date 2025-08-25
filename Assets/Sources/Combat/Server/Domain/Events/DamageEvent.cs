using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.Events
{
    public class DamageEvent : IEvent
    {
        private const float DamagePercentConvertionRation = 0.001f;

        public DamageEvent(Unit attacker, Unit victim, Skill source, float originalDamage, DamageFlags flags)
        {
            Attacker = attacker;
            Victim = victim;
            Source = source;
            OriginalDamage = originalDamage;
            Flags = flags;
            Damage = originalDamage;
            DamagePercent = 100f;

            InProgress = true;
        }

        public Unit Attacker { get; }
        public Unit Victim { get; }
        public Skill Source { get; }
        public float OriginalDamage { get; }
        public DamageFlags Flags { get; set; }
        public float Damage { get; set; }
        public float DamagePercent { get; set; }

        public bool InProgress { get; private set; }

        public void Cancel() => InProgress = false;

        public float GetCurrentDamage() => InProgress ? Damage * DamagePercent * DamagePercentConvertionRation * Attacker.GetAttributeValue(Attributes.Attribute.Versality) / Victim.GetAttributeValue(Attributes.Attribute.Versality) : 0;
    }
}
