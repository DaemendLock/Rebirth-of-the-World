using Combat.Common.ValueObjects;

using System;

using UnityEngine;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly struct MoveInDirectionEffect
    {
        public MoveInDirectionEffect(TransformEffectId id, EntityId target, Vector3 velocity, bool isRelative, float maxDuration)
        {
            Id = id;
            Target = target;
            Velocity = velocity;
            IsRelative = isRelative;
            MaxDuration = maxDuration;
        }

        public TransformEffectId Id { get; }
        public EntityId Target { get; }
        public Vector3 Velocity { get; }
        public bool IsRelative { get; }
        public float MaxDuration { get; }

        public override bool Equals(object obj) => obj is MoveInDirectionEffect effect && Id == effect.Id;
        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
