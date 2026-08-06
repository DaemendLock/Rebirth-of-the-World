using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.Local.Scripting.Capabilities.Skills
{
    public readonly ref struct HandleSkillActionStateChangeCapability
    {
        private readonly ICastStateChangeHandler _handler;

        public HandleSkillActionStateChangeCapability(ICastStateChangeHandler handler)
        {
            _handler = handler;
        }

        public void Handle(ActionState state)
        {
            switch (state)
            {
                case ActionState.Startup:
                    _handler.OnStartup();
                    break;
                case ActionState.Active:
                    _handler.OnActive();
                    break;
                case ActionState.Gap:
                    _handler.OnGapStart();
                    break;
                case ActionState.Recovery:
                    _handler.OnRecovery();
                    break;
                case ActionState.Inactive:
                    _handler.OnEnds();
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
