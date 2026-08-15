using Combat.API;
using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Idk
{
    public sealed class NewScriptStatusPropertyContainer : IStatusPropertyContainer
    {
        private readonly UnitNew _parent;
        private readonly IStatusContext _context;
        private readonly IStatusLifecycleNew _lifecycle;
        private readonly IStatusTickableNew _tickable;

        public NewScriptStatusPropertyContainer(UnitNew parent, IStatusContext context, IStatusScriptNew script)
        {
            _context = context;
            _lifecycle = script as IStatusLifecycleNew;
            _tickable = script as IStatusTickableNew;
        }

        public bool TryGetProperty(out BaseStatusCapabilties effect)
        {
            effect = new(_parent, _context, _lifecycle, _tickable);
            return true;
        }

        public bool TryGetProperty(out HandleIncomingDamageCapability property)
        {
            property = default;
            return false;
        }

        public bool TryGetProperty(out HandleOutgoingDamageCapability property)
        {
            property = default;
            return false;
        }

        public bool TryGetProperty(out ModifyOutgoingDamageCapability property)
        {
            property = default;
            return false;
        }

        public bool TryGetProperty(out ModifyOutgoingHealingCapability property)
        {
            property = default;
            return false;
        }

        public bool TryGetProperty(out ModifyIncomingDamageCapability property)
        {
            property = default;
            return false;
        }

        public bool TryGetProperty(out ModifyAttributesCapability property)
        {
            property = default;
            return false;
        }

        public bool TryGetProperty(out ModifyTimeScaleCapability property)
        {
            property = default;
            return false;
        }
    }
}
