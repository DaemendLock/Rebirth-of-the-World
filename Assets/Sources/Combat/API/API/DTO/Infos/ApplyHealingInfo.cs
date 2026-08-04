using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyHealingInfo
    {
        public readonly float Healing;
        public readonly HealingFlags Flags;
        public readonly IAbilityApi Source;
        public readonly IUnit Healer;

        public ApplyHealingInfo(float healing, HealingFlags flags, IAbilityApi source, IUnit healer)
        {
            Healing = healing;
            Source = source;
            Healer = healer;
            Flags = flags;
        }
    }
}
