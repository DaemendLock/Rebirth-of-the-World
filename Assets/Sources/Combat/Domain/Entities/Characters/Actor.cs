using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.ValueObjects;
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
        public CastActivity CastActivity;

        private readonly List<BreakEffect> _breakEffects;

        public Actor(UnitId id, float baseMovespeed)
        {
            Id = id;
            DesiredActions = new(Vector2.Zero);
            _breakEffects = new List<BreakEffect>();
            BaseMovespeed = baseMovespeed;

            CurrentActivity = ActivityState.Idle;
            CastActivity = default;
        }

        public readonly UnitId Id { get; }

        public ActivityState CurrentActivity { get; private set; }

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

        public readonly bool CanMove => State.HasFlag(ActorState.Rooted) == false && (CurrentActivity.Type == ActivityType.Idle);

        public void StartCast(CastActivity cast)
        {
            CurrentActivity = ActivityState.Casting();
            CastActivity = cast;
        }

        public void StopAction()
        {
            CurrentActivity = ActivityState.Idle;
        }

        public bool TryInterrupt(InterruptReason reason, out ActivityState interruptedAction)
        {
            interruptedAction = CurrentActivity;
            StopAction();

            return interruptedAction.Type switch
            {
                ActivityType.Idle => true,
                _ => true
            };
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
