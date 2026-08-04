using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.API.DTO
{
    public readonly ref struct HitRecord
    {
        public HitRecord(IUnit source, HitboxType hitterType, IUnit target, HurtboxType hurtboxType, Vector3 position)
        {
            Source = source;
            HitboxType = hitterType;
            Target = target;
            HurtboxType = hurtboxType;
            Position = position;
        }

        public IUnit Source { get; }
        public HitboxType HitboxType { get; }
        public IUnit Target { get; }
        public HurtboxType HurtboxType { get; }
        public Vector3 Position { get; }
    }
}
