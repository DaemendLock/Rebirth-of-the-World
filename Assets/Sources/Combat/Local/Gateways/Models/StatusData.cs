using Combat.Common.ValueObjects;
using Combat.Local.API.IDK;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Data.Models
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
            Properties = status.Properties;
        }

        public StatusType Name { get; }
        public AbilityKey? Source { get; }
        public UnitId Parent { get; }
        public int StackCount { get; }
        public Duration Duration { get; }

        public IStatusPropertyContainer Properties { get; }

        public Status ToStatus(StatusId id)
        {
            return new(id, Parent, Name, Source, StackCount, Duration, Properties);
        }
    }
}
