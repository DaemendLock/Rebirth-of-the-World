using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public enum ConsciousState
    {
        Alive,
        KnockedDown,
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
        public Actor(EntityId id, ActorState state, IAction action)
        {
            Id = id;
            State = state;
            CurrentAction = action;
        }

        public EntityId Id { get; }

        public IAction CurrentAction { get; set; }

        public ActorState State { get; set; }

        public bool CanCast => State == ActorState.Silenced == false;

        public bool CanMove => (State.HasFlag(ActorState.Rooted) == false) && (CurrentAction == null || CurrentAction.AllowMovement);

        public void StartAction(IAction action)
        {
            CurrentAction = action;
            action.Start();
        }
    }
}
