using Combat.Local.Scripting.Capabilities.Statuses;

namespace Combat.Local.Scripting.IDK
{
    public interface IStatusPropertyContainer
    {
        bool TryGetProperty(out BaseStatusCapabilties effect);
        bool TryGetProperty(out HandleIncomingDamageCapability property);
        bool TryGetProperty(out HandleOutgoingDamageCapability property);
        bool TryGetProperty(out ModifyOutgoingDamageCapability property);
        bool TryGetProperty(out ModifyOutgoingHealingCapability property);
        bool TryGetProperty(out ModifyIncomingDamageCapability property);
        bool TryGetProperty(out ModifyAttributesCapability property);
        bool TryGetProperty(out ModifyTimeScaleCapability property);
    }
}
