using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public struct Status
    {
        public Status(StatusId id, EntityId parent, StatusName name, EventSource source, int stackCount, Duration duration)
        {
            Id = id;
            Parent = parent;
            Name = name;
            StackCount = stackCount;
            Caster = source.Unit;
            Duration = duration;
            Source = source.Skill;

            StateModifiers = ActorState.None;
        }

        public StatusId Id { get; }
        public EntityId Parent { get; }
        public StatusName Name { get; }
        public EntityId? Caster { get; }
        public SkillId? Source { get; }

        public int StackCount { get; set; }
        public Duration Duration { get; set; }

        public ActorState StateModifiers { get; }
    }
}
