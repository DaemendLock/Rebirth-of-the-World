using Combat.API;
using Combat.API.Contexts;
using Combat.API.Scripting;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public interface IStatusTickCapability
    {
        void Tick(IStatusContext statusContext);
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
