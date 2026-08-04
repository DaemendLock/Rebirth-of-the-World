using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyHealingInfo
    {
        public readonly float Healing;
        public readonly HealingFlags Flags;
        public readonly AbilityApi Source;
        public readonly Unit Healer;

        public ApplyHealingInfo(float healing, HealingFlags flags, AbilityApi source, Unit healer)
        {
            Healing = healing;
            Source = source;
            Healer = healer;
            Flags = flags;
        }
    }
}
