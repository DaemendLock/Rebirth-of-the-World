using Combat.API;
using Combat.API.Scripting;
using Combat.API.Skills;

namespace Combat.Local.Scripting.Idk
{

    public sealed class ApiScriptDrivenAbilityPropertyContainer : IAbilityPropertyContainer
    {
        private readonly SkillScript _script;

        private readonly IHitHandler _hitStrategy;
        private readonly ICastStateChangeHandler _actionStateChangeStrategy;
        private readonly ITargettableSkill _lockTargetStrategy;

        public ApiScriptDrivenAbilityPropertyContainer(SkillScript customSkillStrategy)
        {
            _script = customSkillStrategy;

            if (_script is IHitHandler hitHandler)
            {
                _hitStrategy = hitHandler;
            }

            if (_script is ICastStateChangeHandler actionStateChangeHandler)
            {
                _actionStateChangeStrategy = actionStateChangeHandler;
            }

            if (_script is ITargettableSkill lockTargetHandler)
            {
                _lockTargetStrategy = lockTargetHandler;
            }
        }

        public void Give()
        {
        }

        public void Remove()
        {
        }

        public bool TryGet(out IHitHandler result)
        {
            if (_hitStrategy == null)
            {
                result = default;
                return false;
            }

            result = _hitStrategy;
            return true;
        }

        public bool TryGet(out ICastStateChangeHandler result)
        {
            if (_actionStateChangeStrategy == null)
            {
                result = default;
                return false;
            }

            result = _actionStateChangeStrategy;
            return true;
        }

        public bool TryGet(out ITargettableSkill result)
        {
            if (_lockTargetStrategy == null)
            {
                result = default;
                return false;
            }

            result = _lockTargetStrategy;
            return true;
        }

        //private class DataDrivenActionStateChangeStrategy : ISkillActionStateChangeHandler
        //{
        //    private readonly ICastStateChangeHandler _handler;

        //    public DataDrivenActionStateChangeStrategy(ICastStateChangeHandler handler)
        //    {
        //        _handler = handler;
        //    }

        //    public void Handle(ActionState newState)
        //    {
        //        switch (newState)
        //        {
        //            case ActionState.Startup:
        //                _handler.OnStartup();
        //                break;

        //            case ActionState.Active:
        //                _handler.OnActive();
        //                break;

        //            case ActionState.Gap:
        //                _handler.OnGapStart();
        //                break;

        //            case ActionState.Recovery:
        //                _handler.OnRecovery();
        //                break;

        //            case ActionState.Inactive:
        //                _handler.OnEnds();
        //                break;

        //            default:
        //                throw new System.InvalidOperationException($"Can't find skill state \"{newState}\".");
        //        }
        //    }
        //}
    }
}
