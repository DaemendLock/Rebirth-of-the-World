using Combat.Common;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Gateways.Models
{
    public readonly struct StatusData
    {
        public StatusData(Status status)
        {
            Name = status.Name;
            Source = status.Source;
            Parent = status.Parent;
            StackCount = status.StackCount;
            Duration = status.Duration;
        }

        public StatusType Name { get; }
        public AbilityKey? Source { get; }
        public UnitId Parent { get; }
        public int StackCount { get; }
        public Duration Duration { get; }

        public Status ToStatus(StatusId id)
        {
            return new(id, Parent, Name, Source, StackCount, Duration);
        }
    }
}
