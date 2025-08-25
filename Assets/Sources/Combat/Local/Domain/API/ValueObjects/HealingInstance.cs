using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.API.ValueObjects
{
    public class HealingInstance
    {
        private const float DamagePercentConvertionRation = 0.001f;

        public HealingInstance(Unit healee, Unit healer, ScriptedSkill source, float healing, HealingFlags flags)
        {
            Healer = healer;
            Target = healee;
            Source = source;
            OriginalHealing = healing;
            Flags = flags;
            BaseHealing = healing;
            HealingPercent = 100f;
        }

        public Unit Target { get; }
        public Unit Healer { get; }
        public ScriptedSkill Source { get; }
        public float OriginalHealing { get; }

        public HealingFlags Flags { get; set; }
        public float BaseHealing { get; set; }
        public float HealingPercent { get; set; }

        public float GetCurrentHealing() => BaseHealing * HealingPercent * DamagePercentConvertionRation * Healer.GetAttributeValue(Attribute.Versality) / Target.GetAttributeValue(Attribute.Versality);
    }
}
