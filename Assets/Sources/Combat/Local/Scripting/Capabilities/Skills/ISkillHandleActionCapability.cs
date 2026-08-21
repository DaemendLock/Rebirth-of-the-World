using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.Local.Scripting.Capabilities.Skills
{
    public interface ISkillHandleActionCapability
    {
        void HandlePhaseChange(ICastContext castContext, ISkillContext skillContext, ActionState state);
        void HandleCancel(ICastContext castContext, ISkillContext skillContext);
        void HandleInterrupt(ICastContext castContext, ISkillContext skillContext, InterruptReason reason);
    }

    public sealed class HandleSkillActionStateChangeCapability : ISkillHandleActionCapability
    {
        private readonly ICastStateChangeHandler _handler;

        public HandleSkillActionStateChangeCapability(ICastStateChangeHandler handler)
        {
            _handler = handler;
        }

        public void HandleCancel(ICastContext castContext, ISkillContext skillContext) { }

        public void HandleInterrupt(ICastContext castContext, ISkillContext skillContext, InterruptReason reason) => _handler.OnInterrupt(reason);

        public void HandlePhaseChange(ICastContext castContext, ISkillContext skillContext, ActionState state)
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

    public sealed class NewHandleSkillActionStateChangeCapability : ISkillHandleActionCapability
    {
        private readonly IActableNew _script;

        public NewHandleSkillActionStateChangeCapability(IActableNew handler)
        {
            _script = handler;
        }

        public void HandleCancel(ICastContext castContext, ISkillContext skillContext) => _script.OnCancel(skillContext, castContext);
        public void HandleInterrupt(ICastContext castContext, ISkillContext skillContext, InterruptReason reason) => _script.OnInterrupt(skillContext, castContext, reason);

        public void HandlePhaseChange(ICastContext castContext, ISkillContext skillContext, ActionState state)
        {
            switch (state)
            {
                case ActionState.Startup:
                    _script.OnEnterStartup(skillContext, castContext);
                    return;
                case ActionState.Active:
                    _script.OnEnterActive(skillContext, castContext);
                    return;
                case ActionState.Gap:
                    _script.OnEnterGap(skillContext, castContext);
                    return;
                case ActionState.Recovery:
                    _script.OnEnterRecovery(skillContext, castContext);
                    return;
                case ActionState.Inactive:
                    _script.OnEnded(skillContext, castContext);
                    return;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
