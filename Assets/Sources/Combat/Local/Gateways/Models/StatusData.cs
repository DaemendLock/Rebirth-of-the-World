using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Data.Models
{
    public readonly struct ActorData
    {
        public ActorData(ActorState state, IAction action)
        {
            State = state;
            Action = action;
        }

        public ActorState State { get; }

        public IAction Action { get; }
    }

    public readonly struct StatusData
    {
        public EntityId Parent { get; }
        public StatusName Name { get; }
        public EntityId? Caster { get; }
        public SkillId? Source { get; }

        public int StackCount { get; }
        public Duration Duration { get; }
    }
}
