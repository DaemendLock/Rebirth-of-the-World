using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.API.DTO
{
    public readonly ref struct HitRecord
    {
        public HitRecord(Unit source, HitboxType hitterType, Unit target, HurtboxType hurtboxType, Vector3 position, SkillApi handler)
        {
            Source = source;
            HitboxType = hitterType;
            Target = target;
            HurtboxType = hurtboxType;
            Position = position;
            Handler = handler;
        }

        public Unit Source { get; }
        public HitboxType HitboxType { get; }
        public Unit Target { get; }
        public HurtboxType HurtboxType { get; }
        public Vector3 Position { get; }
        public SkillApi Handler { get; }
    }
}
