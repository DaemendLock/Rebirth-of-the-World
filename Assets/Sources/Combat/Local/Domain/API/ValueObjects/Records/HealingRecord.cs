using Combat.Local.Domain.API.ValueObjects;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct HealingRecord
    {
        public HealingRecord(HealingInstance instance) :
            this(instance.Target, instance.Healer, instance.Source, instance.OriginalHealing, instance.GetCurrentHealing(), instance.Flags)
        { }

        public HealingRecord(Unit target, Unit healer, ScriptedSkill source, float originalHealing, float finalHealing, HealingFlags flags)
        {
            Healer = healer;
            Target = target;
            OriginalHealing = originalHealing;
            FinalHealing = finalHealing;
            Source = source;
            Flags = flags;
        }

        public Unit Healer { get; }
        public Unit Target { get; }
        public float OriginalHealing { get; }
        public float FinalHealing { get; }
        public ScriptedSkill Source { get; }
        public HealingFlags Flags { get; }
    }
}
