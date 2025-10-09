using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public enum ActorState
    {
        Free = 0,
        CantCast = 1,
        CantMove = 2,
    }

    public enum ActionType
    {
        None = 0,
        Casting = 1,
    }

    public readonly struct ActorAction
    {
        public float ActiveTime { get; }
        public ActionType ActionType { get; }
    }

    public ref struct Actor
    {
        private readonly ActorState _state;

        public Actor(EntityId id, ActorState state, IAction action)
        {
            Id = id;
            _state = state;
            CurrentAction = action;
        }

        public EntityId Id { get; }

        public IAction CurrentAction { get; set; }

        public ActorState State => _state;

        public bool IsCasting => CurrentAction != null;

        public bool CanCast => _state == ActorState.CantCast == false;

        public bool CanMove => (_state.HasFlag(ActorState.CantMove) == false) && (CurrentAction == null || CurrentAction.AllowMovement);
    }
}
