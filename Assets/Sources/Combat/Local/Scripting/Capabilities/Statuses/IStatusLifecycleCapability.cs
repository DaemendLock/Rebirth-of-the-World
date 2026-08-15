using Combat.API;
using Combat.API.Contexts;
using Combat.API.Scripting;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public interface IStatusLifecycleCapability
    {
        void Apply(IStatusContext statusContext);
        void Expire(IStatusContext statusContext);
        void Remove(IStatusContext statusContext);
        void Reapply(IStatusContext statusContext);
    }

    public sealed class OldStatusLifecycleCapability : IStatusLifecycleCapability
    {
        private readonly StatusScript _script;

        public OldStatusLifecycleCapability(StatusScript script)
        {
            _script = script;
        }

        public void Apply(IStatusContext statusContext) => _script.OnCreate();
        public void Expire(IStatusContext statusContext) => _script.OnExpire();
        public void Remove(IStatusContext statusContext) => _script.OnRemove();
        public void Reapply(IStatusContext context) { }
    }

    public sealed class NewStatusLifecycleCapability : IStatusLifecycleCapability
    {
        private readonly IStatusScriptNew _script;
        private readonly IActor _parent;

        public NewStatusLifecycleCapability(IStatusScriptNew script, IActor parent)
        {
            _script = script;
            _parent = parent;
        }

        public void Apply(IStatusContext context) => _script.OnApply(_parent, context);
        public void Expire(IStatusContext context) => _script.OnExpire(_parent, context);
        public void Remove(IStatusContext context) => _script.OnRemove(_parent, context);
        public void Reapply(IStatusContext statusContext) { }
    }
}
