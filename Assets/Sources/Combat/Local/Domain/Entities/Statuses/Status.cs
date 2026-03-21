using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public struct Status
    {
        private readonly IStatusStrategy _statusStrategy;

        public Status(StatusId id, EntityId parent, StatusName name, EventSource source, int stackCount, Duration duration, IStatusStrategy statusStrategy)
        {
            Id = id;
            Parent = parent;
            Name = name;
            StackCount = stackCount;
            Caster = source.Unit;
            Duration = duration;
            Source = source.Skill;

            _statusStrategy = statusStrategy;
        }

        public StatusId Id { get; }
        public EntityId Parent { get; }
        public StatusName Name { get; }
        public EntityId? Caster { get; }
        public SkillId? Source { get; }

        public readonly IStatusStrategy Strategy => _statusStrategy;

        public int StackCount { get; set; }
        public Duration Duration { get; set; }

        public void RefreshDuration(float duration)
        {
            Duration = new(Duration.ActiveTime, duration);
        }

        public readonly void Apply() => _statusStrategy.Apply();

        public readonly void Remove() => _statusStrategy.Remove();
    }
}
