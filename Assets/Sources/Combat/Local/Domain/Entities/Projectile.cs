using Combat.Common.Flags;
using Combat.Common.Primitives;

using System;

using UnityEngine;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct Aura
    {
        public readonly AuraId Id { get; }
        public readonly ReadOnlySpan<UnitId> AffectedTargets;
        public readonly AuraTargetFilter TargetFilter;
        public readonly float LingerDuration { get; }
    }

    public ref struct Projectile
    {
        public Projectile(ProjectileId id, UnitId? owner, ModelName modelName, Vector3 position, Vector3 speed)
        {
            Id = id;
            Owner = owner;
            ModelName = modelName;
            Position = position;
            Speed = speed;
        }

        public ProjectileId Id { get; }
        public UnitId? Owner { get; }
        public ModelName ModelName { get; }
        public Vector3 Position { get; set; }
        public Vector3 Speed { get; set; }
    }
}
