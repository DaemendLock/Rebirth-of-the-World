using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.Local.Scripting.Capabilities.Skills
{
    public interface IHandleActionPhaseChangeCapability
    {
        void Handle(ISkillContext skillContext, ICastContext castContext, ActionState state);
    }

    public sealed class HandleActionPhaseChangeCapability : IHandleActionPhaseChangeCapability
    {
        private readonly ICastStateChangeHandler _handler;

        public HandleActionPhaseChangeCapability(ICastStateChangeHandler handler)
        {
            _handler = handler;
        }

        public void Handle(ISkillContext skillContext, ICastContext castContext, ActionState state)
        {
            switch (state)
            {
                case ActionState.Startup:
                    _handler.OnStartup();
                    return;
                case ActionState.Active:
                    _handler.OnActive();
                    return;
                case ActionState.Gap:
                    _handler.OnGapStart();
                    return;
                case ActionState.Recovery:
                    _handler.OnRecovery();
                    return;
                case ActionState.Inactive:
                    _handler.OnEnds();
                    return;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }

    public sealed class NewHandleSkillActionStateChangeCapability : IHandleActionPhaseChangeCapability
    {
        private readonly IActableNew _handler;

        public NewHandleSkillActionStateChangeCapability(IActableNew handler)
        {
            _handler = handler;
        }

        public void Handle(ISkillContext skillContext, ICastContext castContext, ActionState state)
        {
            switch (state)
            {
                case ActionState.Startup:
                    _handler.OnEnterStartup(skillContext, castContext);
                    return;
                case ActionState.Active:
                    _handler.OnEnterActive(skillContext, castContext);
                    return;
                case ActionState.Gap:
                    _handler.OnEnterGap(skillContext, castContext);
                    return;
                case ActionState.Recovery:
                    _handler.OnEnterRecovery(skillContext, castContext);
                    return;
                case ActionState.Inactive:
                    _handler.OnEnded(skillContext, castContext);
                    return;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
