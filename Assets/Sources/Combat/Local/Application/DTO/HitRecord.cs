using Combat.Common.Primitives;
using Combat.Local.Application.Repositories;

using UnityEngine;

namespace Combat.Local.Application.DTO
{
    public readonly ref struct HitRecord
    {
        public HitRecord(HitInfo info) : this(info.HitboxOwner, info.HitboxType, info.HurtboxOwner, info.HurtboxType, info.Location) { }

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

        public HitInfo HitInfo => new(HitboxOwner, HitboxType, HurtboxOwner, HurtboxType, Location);
    }
}
