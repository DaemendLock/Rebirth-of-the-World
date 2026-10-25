using Combat.API.Scripting;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Idk
{
    public sealed class NewScriptStatusCapabilityProvider : IStatusCapabilityProvider
    {
        private readonly IStatusLifecycleCapability _lifecycleCapability;
        private readonly IStatusTickCapability _tickCapability;
        private readonly IStatusModifyMovespeedCapability _modifyMovespeedCapability;

        public NewScriptStatusCapabilityProvider(UnitNew parent, IStatusScriptNew script)
        {
            if (script is IStatusScriptNew lifecycle)
            {
                _lifecycleCapability = new NewStatusLifecycleCapability(lifecycle, parent);
            }

            if (script is IStatusTickableNew tickable)
            {
                _tickCapability = new NewStatusTickCapability(tickable, parent);
            }
        }

        public bool TryGetCapability<T>(out T capability) where T : class
        {
            if (typeof(T) == typeof(IStatusScriptNew))
            {
                capability = _lifecycleCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusTickCapability))
            {
                capability = _tickCapability as T;
                return capability != null;
            }

            capability = null;
            return false;
        }
    }
}
