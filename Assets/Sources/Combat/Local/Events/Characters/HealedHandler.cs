using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Events
{
    public readonly ref struct HealingInfo
    {
        public HealingInfo(EntityId target, float originalHealing, float finalHealing, HealingFlags flags, EntityId? healer, SkillId? skill, EntityId? caster)
        {
            Target = target;
            OriginalHealing = originalHealing;
            FinalHealing = finalHealing;
            Flags = flags;
            Healer = healer;
            Skill = skill;
            Caster = caster;
        }

        public EntityId Target { get; }

        public float OriginalHealing { get; }

        public float FinalHealing { get; }

        public HealingFlags Flags { get; }

        public EntityId? Healer { get; }

        public SkillId? Skill { get; }

        public EntityId? Caster { get; }
    }

    public class HealedHandler : IApplyHealingEventHandler
    {
        public delegate void Handler(HealingInfo info);

        public event Handler Healed;

        public void HandleEvent(HealingInstance instance)
        {
            HealingInfo info = new(instance.Target, instance.OriginalHealing, instance.FinalHealing, instance.Flags, instance.Healer, instance.Source.Skill, instance.Source.Unit);
            Healed?.Invoke(info);
        }
    }
}
