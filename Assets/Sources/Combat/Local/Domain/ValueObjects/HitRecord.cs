using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly struct KillRecord
    {
        public readonly UnitId Victim;

        public KillRecord(UnitId victim)
        {
            Victim = victim;
        }
    }

    public readonly struct HitRecord
    {
        public HitRecord(UnitId hitboxOwner, HitboxType hitboxType, UnitId hurtboxOwner, HurtboxType hurtboxType, Vector3 location)
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
