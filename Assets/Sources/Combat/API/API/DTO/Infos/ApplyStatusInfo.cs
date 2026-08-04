using Combat.Common.ValueObjects;

namespace Combat.API.DTO
{
    public readonly ref struct ApplyStatusInfo
    {
        public readonly IUnit Target;
        public readonly StatusType Name;
        public readonly IAbilityApi Source;
        public readonly float Duration;
        public readonly int StackCount;

        public ApplyStatusInfo(IUnit target, string name, float duration, int stackCount, IAbilityApi source)
        {
            Target = target;
            Name = new(name);
            Source = source;
            Duration = duration;
            StackCount = stackCount;
        }
    }
}
