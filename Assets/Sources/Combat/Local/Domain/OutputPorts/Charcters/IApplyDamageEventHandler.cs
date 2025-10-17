using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.OutputPorts
{
    public interface IApplyDamageEventHandler
    {
        readonly ref struct DamageResult
        {
            public DamageResult(EntityId target, float originalDamage, float finalDamage, DamageFlags flags, EntityId? attacker, SkillId? source, EntityId? caster)
            {
                Target = target;
                OriginalDamage = originalDamage;
                FinalDamage = finalDamage;
                Flags = flags;
                Attacker = attacker;
                Skill = source;
                Caster = caster;
            }

            public EntityId Target { get; }
            public float OriginalDamage { get; }
            public float FinalDamage { get; }
            public DamageFlags Flags { get; }

            public EntityId? Attacker { get; }
            public SkillId? Skill { get; }
            public EntityId? Caster { get; }
        }

        void HandleEvent(DamageResult @event);
    }
}
