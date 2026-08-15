using Combat.API;
using Combat.API.Contexts;
using Combat.API.Scripting;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public readonly ref struct BaseStatusCapabilties
    {
        private readonly StatusScript _statusScript;
        private readonly IStatusLifecycleNew _lifecycle;
        private readonly IStatusTickableNew _tickable;
        private readonly IActor _parent;
        private readonly IStatusContext _context;

        public BaseStatusCapabilties(StatusScript statusScript)
        {
            _statusScript = statusScript;
            _lifecycle = null;
            _tickable = null;
            _parent = null;
            _context = null;
        }

        public BaseStatusCapabilties(IActor parent, IStatusContext context, IStatusLifecycleNew lifecycle, IStatusTickableNew tickable)
        {
            _statusScript = null;
            _lifecycle = lifecycle;
            _tickable = tickable;
            _parent = parent;
            _context = context;
        }

        public bool DestroyOnExpire => true;

        public void Apply()
        {
            _statusScript?.OnCreate();
            _lifecycle?.OnApply(_parent, _context);
        }

        public void Expire()
        {
            _statusScript?.OnExpire();
            _lifecycle?.OnExpire(_parent, _context);
        }

        public void Remove()
        {
            _statusScript?.OnRemove();
            _lifecycle?.OnRemove(_parent, _context);
        }

        public void Tick()
        {
            _statusScript?.OnTick();
            _tickable?.OnTick(_parent, _context);
        }
    }
}
