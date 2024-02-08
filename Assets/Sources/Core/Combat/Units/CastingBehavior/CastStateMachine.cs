using Core.Combat.CastingBehaviors;
using Utils.DataStructure.StateMachine;

namespace Core.Combat.Units.CastingBehaviors
{
    internal class CastStateMachine : IStateMachine<CastingBehavior>
    {
        public CastingBehavior CurrentState { get; private set; } = new Idle();

        public void ChangeState(CastingBehavior state)
        {
            CurrentState.OnExit();
            CurrentState = state;
            state.OnEnter();
        }
    }
}
