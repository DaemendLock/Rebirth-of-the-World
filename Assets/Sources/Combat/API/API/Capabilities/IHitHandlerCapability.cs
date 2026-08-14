using Combat.Common.ValueObjects;

namespace Combat.API.Capabilities
{
    public interface IHitHandlerCapability
    {
        delegate bool HandleHit(int id);

        HitHandlerId Register(HandleHit callback);
    }
}
