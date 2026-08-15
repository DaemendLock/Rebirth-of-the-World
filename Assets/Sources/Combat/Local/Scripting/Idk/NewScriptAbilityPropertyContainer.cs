using Combat.API;
using Combat.API.Scripting;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Idk
{

    public sealed class NewScriptAbilityPropertyContainer : ISkillCapabilityProvider
    {
        private readonly ISkillExecuteCapability _skillExecuteCapability;
        private readonly ISkillHandleActionStateChangeCapability _skillHandleActionStateChangeCapability;

        public NewScriptAbilityPropertyContainer(UnitNew owner, ISkillScriptNew script)
        {
            _skillExecuteCapability = new NewSkillExecuteCapability(script, owner);
            _skillHandleActionStateChangeCapability = new NewHandleSkillActionStateChangeCapability(script, owner);
        }

        public T GetCapability<T>() where T : class
        {
            if (typeof(T) == typeof(ISkillExecuteCapability))
            {
                return _skillExecuteCapability as T;
            }

            if (typeof(T) == typeof(ISkillHandleActionStateChangeCapability))
            {
                return _skillHandleActionStateChangeCapability as T;
            }

            return null;
        }
    }
}
