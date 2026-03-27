using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Entities
{
    public ref struct Positionable
    {
        private readonly Quaternion _rotation;
        private readonly Quaternion _lookDirection;
        private readonly IReadOnlyCollection<MoveInDirectionEffect> _movementEffects;
        private readonly Span<ScaleOverTimeEffect> _scaleEffects;

        public Positionable(EntityId id, Vector3 position, Quaternion rotation, float scale, Quaternion lookDiration, ModelName modelName)
        {
            Id = id;
            ModelName = modelName;
            Scale = scale;

            Position = position;
            _rotation = rotation;
            _lookDirection = lookDiration;
            _movementEffects = Array.Empty<MoveInDirectionEffect>();
            _scaleEffects = Span<ScaleOverTimeEffect>.Empty;
        }

        public Positionable(EntityId id, Vector3 position, Quaternion rotation, float scale, Quaternion lookDiration, ModelName modelName, IReadOnlyCollection<MoveInDirectionEffect> moveEffects)
        {
            Id = id;
            ModelName = modelName;
            Scale = scale;

            Position = position;
            _rotation = rotation;
            _lookDirection = lookDiration;
            _movementEffects = moveEffects;
            _scaleEffects = Span<ScaleOverTimeEffect>.Empty;
        }

        public EntityId Id { get; }

        public Vector3 Position { get; set; }

        public Quaternion Rotation => _rotation;

        public Quaternion LookDirection => _lookDirection;

        public ModelName ModelName { get; }

        public float Scale { get; set; }

        public IReadOnlyCollection<MoveInDirectionEffect> GetMoveInDirectionOverTimeEffects() => _movementEffects;

        public Span<ScaleOverTimeEffect> GetScaleOverTimeEffects() => _scaleEffects;
    }
}
