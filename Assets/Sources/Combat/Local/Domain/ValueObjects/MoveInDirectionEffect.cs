using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly struct MoveInDirectionEffect
    {
        public MoveInDirectionEffect(Vector3 velocity, bool isRelative, float maxDuration)
        {
            Velocity = velocity;
            IsRelative = isRelative;
            MaxDuration = maxDuration;
        }

        public Vector3 Velocity { get; }
        public bool IsRelative { get; }
        public float MaxDuration { get; }
    }
}
