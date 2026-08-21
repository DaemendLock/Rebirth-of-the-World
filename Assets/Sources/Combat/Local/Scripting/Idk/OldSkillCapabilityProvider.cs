using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.IDK;
using Combat.Local.Scripting.Capabilities.Skills;

namespace Combat.Local.Scripting.Idk
{
    public sealed class OldSkillCapabilityProvider : ISkillCapabilityProvider
    {
        private readonly CharacterApiAdapter _characterApiAdapter;

        private readonly ISkillExecuteCapability _skillExecuteCapability;
        private readonly ISkillHitCapability _skillHitHandleCapability;
        private readonly IHandleActionPhaseChangeCapability _skillHandleActionStateChangeCapability;

        private readonly ITargettableSkill _lockTargetStrategy;

        public OldSkillCapabilityProvider(SkillScript script, CharacterApiAdapter characterApiAdapter)
        {
            _characterApiAdapter = characterApiAdapter;

            if (script is ICastableSkill castableSkill)
            {
                _skillExecuteCapability = new OldScriptSkillExecuteContext(castableSkill);
            }

            if (script is ICastStateChangeHandler castStateChangeHandler)
            {
                _skillHandleActionStateChangeCapability = new HandleActionPhaseChangeCapability(castStateChangeHandler);
            }

            if (script is IHitHandler hitHandler)
            {
                _skillHitHandleCapability = new OldHandleSkillHitCapability(hitHandler, characterApiAdapter);
            }

            _lockTargetStrategy = script as ITargettableSkill;
        }

        public T GetCapability<T>() where T : class
        {
            if (typeof(T) == typeof(ISkillExecuteCapability))
            {
                return _skillExecuteCapability as T;
            }

            if (typeof(T) == typeof(ISkillHitCapability))
            {
                return _skillHitHandleCapability as T;
            }

            if (typeof(T) == typeof(IHandleActionPhaseChangeCapability))
            {
                return _skillHandleActionStateChangeCapability as T;
            }

            return null;
        }

        public bool TryGet(out EvaluateSkillTargetCapability result)
        {
            if (_lockTargetStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_lockTargetStrategy, _characterApiAdapter);
            return true;
        }
    }
}
