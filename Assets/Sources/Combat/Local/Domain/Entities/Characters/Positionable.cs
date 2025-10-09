using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct Positionable
    {
        private readonly Vector3 _position;
        private readonly Quaternion _rotation;
        private readonly Quaternion _lookDirection;

        public Positionable(EntityId id, Vector3 position, Quaternion rotation, float scale, Quaternion lookDiration, ModelName modelName)
        {
            Id = id;
            ModelName = modelName;
            Scale = scale;

            _position = position;
            _rotation = rotation;
            _lookDirection = lookDiration;
        }

        public EntityId Id { get; }

        public Vector3 Position => _position;

        public Quaternion Rotation => _rotation;

        public Quaternion LookDirection => _lookDirection;

        public ModelName ModelName { get; }

        public float Scale { get; }
    }
}
