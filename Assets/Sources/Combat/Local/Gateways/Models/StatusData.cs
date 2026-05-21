using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Data.Models
{
    public readonly struct StatusData
    {
        public StatusData(Status status)
        {
            Parent = status.Parent;
            Name = status.Name;
            Source = status.Source;
            StackCount = status.StackCount;
            Duration = status.Duration;
            Properties = status.Properties;
        }

        public UnitId Parent { get; }
        public StatusType Name { get; }
        public AbilityKey? Source { get; }

        public int StackCount { get; }
        public Duration Duration { get; }

        public IStatusPropertyContainer Properties { get; }

        public Status ToStatus(StatusId id)
        {
            return new(id, Parent, Name, Source, StackCount, Duration, Properties);
        }
    }
}
