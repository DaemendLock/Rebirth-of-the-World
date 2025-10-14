using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Events
{
    public readonly ref struct DamageInfo
    {
        public DamageInfo(EntityId target, float originalDamage, float finalDamage, DamageFlags flags, EntityId? attacker, SkillId? source, EntityId? caster)
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

    public class DamagedHandler : IApplyDamageEventHandler
    {
        public delegate void Handler(DamageInfo damageInfo);

        public event Handler Damaged;

        public void HandleEvent(DamageResult instance)
        {
            DamageInfo damageInfo = new(instance.Target, instance.OriginalDamage, instance.FinalDamage, instance.Flags, instance.Attacker, instance.Skill, instance.Caster);
            Damaged?.Invoke(damageInfo);
        }
    }
}
