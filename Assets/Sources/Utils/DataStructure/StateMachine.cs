namespace Utils.DataStructure.StateMachine
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
    }

    public interface IStateMachine<T> where T : IState
    {
        public T CurrentState { get; }

        public void ChangeState(T state);
    }
}
