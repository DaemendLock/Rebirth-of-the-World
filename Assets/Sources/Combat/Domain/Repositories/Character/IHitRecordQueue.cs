using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Application.Repositories
{
    public readonly struct HitInfo
    {
        public HitInfo(UnitId hitboxOwner, HitboxType hitboxType, UnitId hurtboxOwner, HurtboxType hurtboxType, Vector3 location)
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

    public interface IHitRecordQueue
    {
        void Enqueue(HitInfo record);
        bool TryDequeue(out HitInfo record);
    }
}
