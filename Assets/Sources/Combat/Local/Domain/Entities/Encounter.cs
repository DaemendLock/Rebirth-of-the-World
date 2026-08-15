using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public interface IEncounterStateMachine
    {
        EncounterState State { get; }
        bool IsRunning { get; }

        bool TryStart();
        bool TryPause();
        bool TryResume();
        bool TryBeginFinalize();
        bool TryFinalize(EncounterState targetState);
    }

    public sealed class EncounterStateMachine : IEncounterStateMachine
    {
        public EncounterState State { get; private set; } = EncounterState.Starting;

        public bool IsRunning => State == EncounterState.Running;

        public bool TryStart() => TryTransition(EncounterState.Starting, EncounterState.Running);

        public bool TryPause() => TryTransition(EncounterState.Running, EncounterState.Paused);

        public bool TryResume() => TryTransition(EncounterState.Paused, EncounterState.Running);

        public bool TryBeginFinalize()
        {
            switch (State)
            {
                case EncounterState.Starting:
                case EncounterState.Running:
                case EncounterState.Paused:
                    State = EncounterState.Ending;
                    return true;
                case EncounterState.Ending:
                case EncounterState.Completed:
                case EncounterState.Failed:
                case EncounterState.Cancelled:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(State), State, null);
            }
        }

        public bool TryFinalize(EncounterState targetState) => TryTransition(EncounterState.Ending, targetState);

        private bool TryTransition(EncounterState expected, EncounterState target)
        {
            if (State != expected)
            {
                return false;
            }

            State = target;
            return true;
        }
    }
}
