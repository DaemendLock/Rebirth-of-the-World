using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

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

        public void Progreess(float time) => Duration = Duration.Progress(time);

        public void RefreshDuration(float duration) => Duration = new(0, duration);
    }
}
