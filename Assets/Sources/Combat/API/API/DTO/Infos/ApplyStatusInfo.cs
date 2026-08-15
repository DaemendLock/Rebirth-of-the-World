using Combat.Common.ValueObjects;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyStatusInfo
    {
        public readonly Unit Target;
        public readonly StatusType Name;
        public readonly AbilityApi Source;
        public readonly float Duration;
        public readonly int StackCount;

        public ApplyStatusInfo(Unit target, string name, float duration, int stackCount, AbilityApi source)
        {
            Target = target;
            Name = new(name);
            Source = source;
            Duration = duration;
            StackCount = stackCount;
        }
    }
}
