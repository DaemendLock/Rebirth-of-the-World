using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct HitRecord
    {
        public HitRecord(Unit source, HitboxType hitterType, Unit target, HurtboxType hurtboxType, Vector3 position)
        {
            Source = source;
            HitboxType = hitterType;
            Target = target;
            HurtboxType = hurtboxType;
            Position = position;
        }

        public Unit Source { get; }
        public HitboxType HitboxType { get; }
        public Unit? Target { get; }
        public HurtboxType HurtboxType { get; }
        public Vector3 Position { get; }
    }
}
