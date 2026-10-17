using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.Domain.Entities
{
    public readonly struct StatusInstance
    {
        public StatusInstance(StatusId id, StatusType name, Duration duration, int stackCount, AbilityKey? source)
        {
            Id = id;
            Duration = duration;
            StackCount = stackCount;
            Name = name;
            Source = source;
        }

        public StatusId Id { get; }
        public StatusType Name { get; }
        public Duration Duration { get; }
        public int StackCount { get; }
        public AbilityKey? Source { get; }

        public StatusInstance Progress(float deltaTime) => new(Id, Name, Duration.Progress(deltaTime), StackCount, Source);

        public StatusInstance WithDuration(Duration duration) => new(Id, Name, duration, StackCount, Source);

        public StatusInstance WithStackCount(int stackCount) => new(Id, Name, Duration, stackCount, Source);
    }
}
