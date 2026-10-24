using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.ValueObjects;

using System.Numerics;

namespace Combat.Domain.Entities
{
    public struct Actor
    {
        public Actor(UnitId id)
        {
            Id = id;
            State = default;
            CurrentAction = null;
            DesiredActions = new(default, Vector2.Zero);
        }

        public readonly UnitId Id { get; }

        public Action CurrentAction { readonly get; private set; }

        public DesiredActions DesiredActions { readonly get; private set; }

        public ActorState State { get; set; }

        public readonly bool CanCast => State.HasFlag(ActorState.Silenced) == false;

        public readonly bool CanMove => State.HasFlag(ActorState.Rooted) == false && (CurrentAction == null || CurrentAction.AllowMovement);

        public void StartAction(Action action)
        {
            CurrentAction = action;
            action.Start();
        }

        public void StopAction()
        {
            CurrentAction = null;
        }

        public bool TryInterrupt(InterruptReason reason) => TryInterrupt(reason, out _);

        public bool TryInterrupt(InterruptReason reason, out Action interruptedAction)
        {
            interruptedAction = CurrentAction;
            StopAction();
            DesireCast(null);

            if (interruptedAction == null)
            {
                return false;
            }

            interruptedAction.Interrupt(reason);
            return true;
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
