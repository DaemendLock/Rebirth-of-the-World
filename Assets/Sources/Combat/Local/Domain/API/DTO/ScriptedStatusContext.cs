using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct ScriptedStatusContext
    {
        public readonly StatusId Id;
        public readonly Unit Parent;
        public readonly StatusName Name;
        //public readonly Enviroment Enviroment;
        public readonly ScriptedSkill Source;
        public readonly Duration Duration;
        public readonly int StackCount;

        public ScriptedStatusContext(StatusId id, StatusName name, Unit parent, ScriptedSkill source, Duration duration, int stackCount)
        {
            Id = id;
            Parent = parent;
            Source = source;
            Duration = duration;
            StackCount = stackCount;
            Name = name;
        }
    }
}
