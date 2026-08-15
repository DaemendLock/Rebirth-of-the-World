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
    }

    public interface IStatusTickCapability
    {
        void Tick(IStatusContext statusContext);
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
    }

    public sealed class OldStatusTickCapability : IStatusTickCapability
    {
        private readonly StatusScript _script;

        public OldStatusTickCapability(StatusScript script)
        {
            _script = script;
        }

        public void Tick(IStatusContext context) => _script.OnTick();
    }

    public sealed class NewStatusLifecycleCapability : IStatusLifecycleCapability
    {
        private readonly IStatusLifecycleNew _script;
        private readonly IActor _parent;

        public NewStatusLifecycleCapability(IStatusLifecycleNew script, IActor parent)
        {
            _script = script;
            _parent = parent;
        }

        public void Apply(IStatusContext context) => _script.OnApply(_parent, context);
        public void Expire(IStatusContext context) => _script.OnExpire(_parent, context);
        public void Remove(IStatusContext context) => _script.OnRemove(_parent, context);
    }

    public sealed class NewStatusTickCapability : IStatusTickCapability
    {
        private readonly IStatusTickableNew _script;
        private readonly IActor _parent;

        public NewStatusTickCapability(IStatusTickableNew script, IActor parent)
        {
            _script = script;
            _parent = parent;
        }

        public void Tick(IStatusContext context) => _script.OnTick(_parent, context);
    }
}
