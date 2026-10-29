using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.ValueObjects;

using System.Collections.Generic;
using System.Numerics;

namespace Combat.Domain.Entities
{
    public struct BreakEffect
    {
        public ActorState State;
        public float Duration;

        public BreakEffect(ActorState state, float duration)
        {
            State = state;
            Duration = duration;
        }
    }

    public struct Actor
    {
        private readonly List<BreakEffect> _breakEffects;

        public Actor(UnitId id, float baseMovespeed)
        {
            Id = id;
            CurrentAction = null;
            DesiredActions = new(Vector2.Zero);
            _breakEffects = new List<BreakEffect>();
            BaseMovespeed = baseMovespeed;
        }

        public readonly UnitId Id { get; }

        public Action CurrentAction { readonly get; private set; }

        public DesiredActions DesiredActions { readonly get; private set; }

        public float BaseMovespeed { get; }

        public readonly ActorState State
        {
            get
            {
                ActorState state = ActorState.None;

                foreach (BreakEffect breakEffect in _breakEffects)
                {
                    state |= breakEffect.State;
                }

                return state;
            }
        }

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

            if (interruptedAction == null)
            {
                return false;
            }

            interruptedAction.Interrupt(reason);
            return true;
        }

        public void DesireMoveDirection(Vector2 direction)
        {
            DesiredActions = new(direction);
        }

        public readonly void ApplyBreak(ActorState type, float duration)
        {
            _breakEffects.Add(new(type, duration));
        }

        public readonly void EndBreak()
        {
            _breakEffects.Clear();
        }
    }
}
