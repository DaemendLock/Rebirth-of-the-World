using Combat.Common.ValueObjects;

namespace Combat.API.DTO
{
    public readonly ref struct StatusApplicationData
    {
        public readonly StatusName Name;
        public readonly SkillApi Source;
        public readonly float Duration;
        public readonly int StackCount;

        public StatusApplicationData(string name, SkillApi source, float duration, int stackCount)
        {
            Name = new(name);
            Source = source;
            Duration = duration;
            StackCount = stackCount;
        }
    }
}
