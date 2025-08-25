using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct StatusApplicationData
    {
        public readonly StatusName Name;
        public readonly ScriptedSkill Source;
        public readonly float Duration;
        public readonly int StackCount;

        public StatusApplicationData(string name, ScriptedSkill source, float duration, int stackCount)
        {
            Name = new(name);
            Source = source;
            Duration = duration;
            StackCount = stackCount;
        }
    }
}
