using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.Events
{
    public class HealingEvent : IEvent
    {
        private const float DamagePercentConvertionRation = 0.001f;

        public HealingEvent(Unit healer, Unit healee, Skill source, float healing, HealingFlags flags)
        {
            Healer = healer;
            Healee = healee;
            Source = source;
            OriginalHealing = healing;
            Flags = flags;
            Healing = healing;
            HealingPercent = 100f;

            InProgress = true;
        }

        public Unit Healer { get; }
        public Unit Healee { get; }
        public Skill Source { get; }
        public float OriginalHealing { get; }
        public HealingFlags Flags { get; set; }
        public float Healing { get; set; }
        public float HealingPercent { get; set; }

        public bool InProgress { get; private set; }

        public void Cancel() => InProgress = false;

        public float GetCurrentHealing() => InProgress ? Healing * HealingPercent * DamagePercentConvertionRation * Healer.GetAttributeValue(Attributes.Attribute.Versality) / Healee.GetAttributeValue(Attributes.Attribute.Versality) : 0;
    }
}
