using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System.Numerics;

namespace Combat.Local.Domain.Entities
{
    public ref struct Actor
    {
        public Actor(UnitId id, ActorState state, Action action, ConsciousState consciousState, DesiredActions desiredActions)
        {
            Id = id;
            State = state;
            CurrentAction = action;
            ConsciousState = consciousState;
            DesiredActions = desiredActions;
        }

        public readonly UnitId Id { get; }

        public Action CurrentAction { readonly get; private set; }

        public DesiredActions DesiredActions { readonly get; private set; }

        public ConsciousState ConsciousState { readonly get; private set; }

        public ActorState State { get; set; }

        public readonly bool CanCast => State.HasFlag(ActorState.Silenced) == false;

        public readonly bool CanMove => (State.HasFlag(ActorState.Rooted) == false) && (CurrentAction == null || CurrentAction.AllowMovement) && (ConsciousState != ConsciousState.Dead);

        public void StartAction(Action action)
        {
            CurrentAction = action;
            action.Start();
        }

        public void StopAction()
        {
            CurrentAction = null;
        }

        public void Kill()
        {
            ConsciousState = ConsciousState.Dead;
            CurrentAction?.Interrupt(InterruptReason.Death);
        }

        public void Revive()
        {
            ConsciousState = ConsciousState.Alive;
        }

        public void DesireCast(SkillId? skillId)
        {
            DesiredActions = new(skillId, DesiredActions.MovementDirection);
        }

        public void DesireMoveDirection(Vector2 direction)
        {
            DesiredActions = new(DesiredActions.Skill, direction);
        }
    }
}
