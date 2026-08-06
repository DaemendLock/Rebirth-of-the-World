using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.IDK;
using Combat.Local.Scripting.Idk.Capabilities.Skills;

namespace Combat.Local.Scripting.Idk
{
    public sealed class ApiScriptDrivenAbilityPropertyContainer : IAbilityPropertyContainer
    {
        private readonly CharacterApiAdapter _characterApiAdapter;

        private readonly ICastableSkill _castableSkill;
        private readonly IHitHandler _hitStrategy;
        private readonly ICastStateChangeHandler _actionStateChangeStrategy;
        private readonly ITargettableSkill _lockTargetStrategy;

        public ApiScriptDrivenAbilityPropertyContainer(SkillScript script, CharacterApiAdapter characterApiAdapter)
        {
            _characterApiAdapter = characterApiAdapter;

            _hitStrategy = script as IHitHandler;
            _actionStateChangeStrategy = script as ICastStateChangeHandler;
            _lockTargetStrategy = script as ITargettableSkill;
            _castableSkill = script as ICastableSkill;
        }

        public bool TryGet(out HandleSkillHitCapability result)
        {
            if (_hitStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_hitStrategy, _characterApiAdapter);
            return true;
        }

        public bool TryGet(out HandleSkillActionStateChangeCapability result)
        {
            if (_actionStateChangeStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_actionStateChangeStrategy);
            return true;
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

        public bool TryGet(out ExecuteSkillCapability result)
        {
            if (_castableSkill == null)
            {
                result = default;
                return false;
            }

            result = new(_castableSkill);
            return true;
        }
    }
}
