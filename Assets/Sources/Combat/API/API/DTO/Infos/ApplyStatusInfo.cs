using Combat.Common.Primitives;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyStatusInfo
    {
        public readonly UnitId Target;
        public readonly StatusType Name;
        public readonly AbilityKey? Source;
        public readonly float Duration;
        public readonly int StackCount;

        public ApplyStatusInfo(UnitId target, string name, float duration, int stackCount, AbilityKey? source)
        {
            Target = target;
            Name = new(name);
            Source = source;
            Duration = duration;
            StackCount = stackCount;
        }
    }
}
