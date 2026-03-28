using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public enum ConsciousState
    {
        Alive,
        Dead,
    }

    [Flags]
    public enum ActorState
    {
        None = 0,
        Silenced = 1,
        Rooted = 2,
    }

    public enum ActionType
    {
        None = 0,
        Casting = 1,
    }

    public ref struct Actor
    {
        public Actor(EntityId id, ActorState state, Action action)
        {
            Id = id;
            State = state;
            CurrentAction = action;
        }

        public EntityId Id { get; }

        public Action CurrentAction { get; set; }

        public ActorState State { get; set; }

        public readonly bool CanCast => State.HasFlag(ActorState.Silenced) == false;

        public readonly bool CanMove => (State.HasFlag(ActorState.Rooted) == false) && (CurrentAction == null || CurrentAction.AllowMovement);

        public void StartAction(Action action)
        {
            CurrentAction = action;
            action.Start();
        }
    }
}
