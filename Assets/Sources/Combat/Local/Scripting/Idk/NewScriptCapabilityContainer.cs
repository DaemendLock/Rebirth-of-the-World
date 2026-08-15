using Combat.API.Scripting;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Idk
{
    public sealed class NewScriptCapabilityContainer : ISkillCapabilityProvider
    {
        private readonly ISkillExecuteCapability _skillExecuteCapability;
        private readonly ISkillHandleActionStateChangeCapability _skillHandleActionStateChangeCapability;

        public NewScriptCapabilityContainer(UnitNew owner, ISkillScriptNew script)
        {
            if (script is ICastableNew castable)
            {
                _skillExecuteCapability = new NewSkillExecuteCapability(castable, owner);
            }

            if (script is IActableNew actable)
            {
                _skillHandleActionStateChangeCapability = new NewHandleSkillActionStateChangeCapability(actable, owner);
            }
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
