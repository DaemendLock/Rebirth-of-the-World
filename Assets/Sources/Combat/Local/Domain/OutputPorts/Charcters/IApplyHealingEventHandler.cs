using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.OutputPorts
{
    public interface IApplyHealingEventHandler
    {
        readonly ref struct HealingResult
        {
            public HealingResult(EntityId target, float originalHealing, float healing, HealingFlags flags, EntityId? healer, SkillId? skill, EntityId? caster)
            {
                Target = target;
                OriginalHealing = originalHealing;
                Healing = healing;
                Flags = flags;
                Healer = healer;
                Skill = skill;
                Caster = caster;
            }

            public EntityId Target { get; }
            public float OriginalHealing { get; }
            public float Healing { get; }
            public HealingFlags Flags { get; }
            public EntityId? Healer { get; }
            public SkillId? Skill { get; }
            public EntityId? Caster { get; }
        }

        void HandleEvent(HealingResult instance);
    }
}
