using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.DTO
{
    public readonly ref struct HealingData
    {
        public readonly float Healing;
        public readonly Skill Source;
        public readonly Unit Healer;
        public readonly HealingFlags Flags;

        public HealingData(Unit healer, Skill source, float healing, HealingFlags flags)
        {
            Healing = healing;
            Source = source;
            Healer = healer;
            Flags = flags;
        }
    }
}
