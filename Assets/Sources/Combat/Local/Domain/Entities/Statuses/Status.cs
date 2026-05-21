using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public ref struct Status
    {
        private readonly IStatusPropertyContainer _propertyContainer;

        public Status(StatusId id, UnitId parent, StatusType name, AbilityKey? source, int stackCount, Duration duration, IStatusPropertyContainer propertyContainer)
        {
            Id = id;
            Parent = parent;
            Name = name;
            StackCount = stackCount;
            Duration = duration;
            Source = source;

            _propertyContainer = propertyContainer;
        }

        public StatusId Id { get; }
        public StatusType Name { get; }
        public UnitId Parent { get; }
        public AbilityKey? Source { get; }

        public readonly IStatusPropertyContainer Properties => _propertyContainer;

        public int StackCount { get; set; }
        public Duration Duration { get; set; }

        public void RefreshDuration(float duration)
        {
            Duration = new(Duration.ActiveTime, duration);
        }
    }
}
