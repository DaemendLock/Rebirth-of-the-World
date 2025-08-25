namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct HealingData
    {
        public readonly float Healing;
        public readonly ScriptedSkill Source;
        public readonly Unit Healer;
        public readonly HealingFlags Flags;

        public HealingData(Unit healer, ScriptedSkill source, float healing, HealingFlags flags)
        {
            Healing = healing;
            Source = source;
            Healer = healer;
            Flags = flags;
        }
    }
}
