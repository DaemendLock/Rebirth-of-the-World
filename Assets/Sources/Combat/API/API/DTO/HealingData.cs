using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct HealingData
    {
        public readonly float Healing;
        public readonly SkillApi Source;
        public readonly Unit Healer;
        public readonly HealingFlags Flags;

        public HealingData(Unit healer, SkillApi source, float healing, HealingFlags flags)
        {
            Healing = healing;
            Source = source;
            Healer = healer;
            Flags = flags;
        }
    }
}
