using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Entities
{
    public ref struct Positionable
    {
        private readonly Quaternion _lookDirection;

        public Positionable(EntityId id, Vector3 position, Quaternion rotation, float scale, Quaternion lookDiration, ModelName modelName)
        {
            Id = id;
            ModelName = modelName;
            Scale = scale;

            Position = position;
            Rotation = rotation;
            _lookDirection = lookDiration;
        }

        public EntityId Id { get; }

        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public Quaternion LookDirection => _lookDirection;

        public ModelName ModelName { get; }

        public float Scale { get; set; }
    }
}
