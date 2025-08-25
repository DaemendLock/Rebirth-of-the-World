using System;

using Utils.Patterns.StateMachine.FinitStateMachine;

namespace Utils.Patterns.StateMachine.Implementations
{
    public class FuncTransition<T> : ITransition<T> where T : IState
    {
        private readonly Func<bool> _canTransit;

        public FuncTransition(Func<bool> canTransit, T nextState)
        {
            _canTransit = canTransit;
            NextState = nextState;
        }

        public bool CanTransit => _canTransit.Invoke();

        public T NextState { get; }
    }
}
