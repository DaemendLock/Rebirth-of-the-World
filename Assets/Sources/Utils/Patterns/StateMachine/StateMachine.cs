namespace Utils.Patterns.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
    }

    public interface IStateMachine<out T> where T : IState
    {
        public T CurrentState { get; }
    }
}
