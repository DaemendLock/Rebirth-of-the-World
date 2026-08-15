using Combat.API;
using Combat.API.Scripting;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Idk
{
    public sealed class NewScriptStatusCapabilityProvider : IStatusCapabilityProvider
    {
        private readonly IStatusLifecycleCapability _lifecycleCapability;
        private readonly IStatusTickCapability _tickCapability;

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

        public T GetCapability<T>() where T : class
        {
            if (typeof(T) == typeof(IStatusScriptNew))
            {
                return _lifecycleCapability as T;
            }

            if (typeof(T) == typeof(IStatusTickCapability))
            {
                return _tickCapability as T;
            }

            return null;
        }
    }
}
