using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.Entities
{
    public ref struct Positionable
    {
        public Positionable(UnitId id, Vector3 position, Quaternion rotation, float scale, Quaternion lookDiration, ModelName modelName, Vector3 velocity)
        {
            Id = id;
            ModelName = modelName;
            Scale = scale;

            Position = position;
            Rotation = rotation;
            LookDirection = lookDiration;
            Velocity = velocity;
        }

        public UnitId Id { get; }

        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public Quaternion LookDirection { get; set; }

        public ModelName ModelName { get; }

        public Vector3 Velocity { get; set; }

        public float Scale { get; set; }
    }
}
