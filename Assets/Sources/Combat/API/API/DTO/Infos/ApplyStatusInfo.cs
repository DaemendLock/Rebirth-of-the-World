using Combat.Common.ValueObjects;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyStatusInfo
    {
        public readonly StatusName Name;
        public readonly SkillApi Source;
        public readonly float Duration;
        public readonly int StackCount;

        public ApplyStatusInfo(string name, float duration, int stackCount, SkillApi source)
        {
            Name = new(name);
            Source = source;
            Duration = duration;
            StackCount = stackCount;
        }
    }
}
