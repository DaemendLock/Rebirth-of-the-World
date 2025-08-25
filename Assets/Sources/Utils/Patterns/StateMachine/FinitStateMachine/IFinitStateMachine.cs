using System.Collections.Generic;

namespace Utils.Patterns.StateMachine.FinitStateMachine
{
    public interface IFinitStateMachine<out T> : IStateMachine<T> where T : IState
    {
        void Run();
        void Stop();
    }

    public interface IStateChanger<T> where T : IState
    {
        void Change(T state);
    }

    public interface ITransition<out T> where T : IState
    {
        bool CanTransit { get; }
        T NextState { get; }
    }

    public interface IStateCollection<T> where T : IState
    {
        bool HasState(T state);
        IEnumerable<ITransition<T>> GetTransitions(T state);
    }
}
