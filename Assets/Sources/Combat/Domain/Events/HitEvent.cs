using Combat.Common.Primitives;

using System.Numerics;

namespace Combat.Domain.Events
{
    public readonly struct HitEvent
    {
        public HitEvent(UnitId hitboxOwner, HitboxType hitboxType, UnitId hurtboxOwner, HurtboxType hurtboxType, Vector3 location)
        {
            HitboxOwner = hitboxOwner;
            HitboxType = hitboxType;
            HurtboxOwner = hurtboxOwner;
            HurtboxType = hurtboxType;
            Location = location;
        }

        public UnitId HitboxOwner { get; }
        public HitboxType HitboxType { get; }

        public UnitId HurtboxOwner { get; }
        public HurtboxType HurtboxType { get; }

        public Vector3 Location { get; }
    }
}
