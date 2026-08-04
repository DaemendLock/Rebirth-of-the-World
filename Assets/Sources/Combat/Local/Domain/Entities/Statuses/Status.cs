using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public ref struct Status
    {
        public Status(StatusId id, UnitId parent, StatusType name, AbilityKey? source, int stackCount, Duration duration)
        {
            Id = id;
            Parent = parent;
            Name = name;
            StackCount = stackCount;
            Duration = duration;
            Source = source;
        }

        public StatusId Id { get; }
        public StatusType Name { get; }
        public UnitId Parent { get; }
        public AbilityKey? Source { get; }

        public int StackCount { get; set; }
        public Duration Duration { get; set; }

        public readonly void Progreess(float time)
        {
            Duration.Progress(time);
        }

        public void RefreshDuration(float duration)
        {
            Duration = new(Duration.ActiveTime, duration);
        }
    }
}
