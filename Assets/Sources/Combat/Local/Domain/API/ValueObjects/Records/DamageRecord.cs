using Combat.Local.Domain.API.ValueObjects;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct DamageRecord
    {
        public DamageRecord(DamageInstance instance) : this(instance.Target, instance.Attacker, instance.Source, instance.OriginalDamage, instance.GetCurrentDamage(), instance.Flags)
        { }

        public DamageRecord(Unit target, Unit attacker, ScriptedSkill source, float originalDamage, float finalDamage, DamageFlags flags)
        {
            Attacker = attacker;
            Target = target;
            OriginalDamage = originalDamage;
            FinalDamage = finalDamage;
            Source = source;
            Flags = flags;
        }

        public Unit Target { get; }
        public Unit Attacker { get; }
        public float OriginalDamage { get; }
        public float FinalDamage { get; }
        public ScriptedSkill Source { get; }
        public DamageFlags Flags { get; }
    }
}
