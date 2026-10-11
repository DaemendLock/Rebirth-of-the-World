using Combat.Common.Primitives;

using System.Numerics;

namespace Combat.API.Capabilities
{
    public readonly struct HitHandleOptions
    {
        public readonly UnitId Source;
    }

    public readonly struct HitContext
    {
        public readonly UnitId Source;
        public readonly UnitId? Victim;
        public readonly Vector3 Position;

        public readonly HitboxType HitboxType;
        public readonly HurtboxType HurtboxType;
    }

    public interface IHandleHitsCapability
    {
        delegate void HandleHit(HitContext context);

        HitHandlerId ListenToHits(UnitId owner, HandleHit callback);
        void StopListenToHits(HitHandlerId hitHandlerId);
    }
}
