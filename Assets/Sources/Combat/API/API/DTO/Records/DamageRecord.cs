using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct DamageRecord
    {
        public DamageRecord(IUnit target, float originalDamage, float finalDamage, DamageFlags flags, IUnit attacker, IAbilityApi source)
        {
            Attacker = attacker;
            Target = target;
            OriginalDamage = originalDamage;
            FinalDamage = finalDamage;
            Source = source;
            Flags = flags;
        }

        public IUnit Target { get; }
        public IUnit Attacker { get; }
        public float OriginalDamage { get; }
        public float FinalDamage { get; }
        public IAbilityApi Source { get; }
        public DamageFlags Flags { get; }
    }
}
