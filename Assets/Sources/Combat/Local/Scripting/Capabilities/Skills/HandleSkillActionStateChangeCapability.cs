using Combat.API;
using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.Local.Scripting.Capabilities.Skills
{
    public interface ISkillHandleActionStateChangeCapability
    {
        void Handle(ISkillContext skillContext, ActionState state);
    }

    public sealed class HandleSkillActionStateChangeCapability : ISkillHandleActionStateChangeCapability
    {
        private readonly ICastStateChangeHandler _handler;

        public HandleSkillActionStateChangeCapability(ICastStateChangeHandler handler)
        {
            _handler = handler;
        }

        public void Handle(ISkillContext skillContext, ActionState state)
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

    public sealed class NewHandleSkillActionStateChangeCapability : ISkillHandleActionStateChangeCapability
    {
        private readonly UnitNew _actor;
        private readonly ISkillScriptNew _handler;

        public NewHandleSkillActionStateChangeCapability(ISkillScriptNew handler, UnitNew actor)
        {
            _actor = actor;
            _handler = handler;
        }

        public void Handle(ISkillContext skillContext, ActionState state)
        {
            switch (state)
            {
                case ActionState.Startup:
                    _handler.OnEnterStartup(_actor, skillContext);
                    return;
                case ActionState.Active:
                    _handler.OnEnterActive(_actor, skillContext);
                    return;
                case ActionState.Recovery:
                    _handler.OnEnterRecovery(_actor, skillContext);
                    return;
                case ActionState.Inactive:
                case ActionState.Gap:
                    return;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
