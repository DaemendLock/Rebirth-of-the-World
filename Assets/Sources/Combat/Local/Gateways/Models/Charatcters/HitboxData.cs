using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    public readonly struct NewHitData
    {
        public NewHitData(UnitId hitboxOwner, HitboxType hitboxType, UnitId hurtboxOwner, HurtboxType hurtboxType, Vector3 location)
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

    public readonly struct HitboxData
    {
        public HitboxData(HitboxType type, UnitId owner)
        {
            Type = type;
            Owner = owner;
        }

        public HitboxType Type { get; }

        public UnitId Owner { get; }
    }

    public readonly struct HurtboxData
    {
        public HurtboxData(HurtboxType type, UnitId owner)
        {
            Type = type;
            Owner = owner;
        }

        public HurtboxType Type { get; }

        public UnitId Owner { get; }
    }
}
