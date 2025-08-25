using System.Collections.Generic;
using System.Linq;

using Utils.Patterns.StateMachine.FinitStateMachine;

namespace Utils.Patterns.StateMachine.Implementations
{
    public class SeparatedTransitionsStateCollection<T> : IStateCollection<T> where T : IState
    {
        private readonly Dictionary<T, IEnumerable<ITransition<T>>> _states;

        public SeparatedTransitionsStateCollection()
        {
            _states = new();
        }

        public void Add(T state) => _states[state] = Enumerable.Empty<ITransition<T>>();
        public void SetTransitions(T state, IEnumerable<ITransition<T>> transitions) => _states[state] = transitions;
        public IEnumerable<ITransition<T>> GetTransitions(T state) => _states.GetValueOrDefault(state, Enumerable.Empty<ITransition<T>>());
        public bool HasState(T state) => _states.ContainsKey(state);
    }
}
