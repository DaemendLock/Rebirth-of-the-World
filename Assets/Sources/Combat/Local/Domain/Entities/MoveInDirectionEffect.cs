using Combat.Common.ValueObjects;

using System;

using UnityEngine;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly struct MoveInDirectionEffect
    {
        public MoveInDirectionEffect(EntityId target, Vector3 velocity, bool isRelative)
        {
            Id = 0;
            Target = target;
            Velocity = velocity;
            IsRelative = isRelative;
        }

        public int Id { get; }
        public EntityId Target { get; }
        public Vector3 Velocity { get; }
        public bool IsRelative { get; }

        public override bool Equals(object obj) => obj is MoveInDirectionEffect effect && Id == effect.Id;
        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
