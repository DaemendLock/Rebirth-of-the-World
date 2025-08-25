using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.Entities.Units
{
    public struct Transform
    {
        public Transform(EntityId id, ModelName modelId, Team team)
        {
            Id = id;
            ModelName = modelId;
            Team = team;
            Position = default;
            Rotation = 0;
            Scale = 1;
        }

        public EntityId Id { get; }

        public ModelName ModelName { get; set; }

        public Team Team { get; set; }

        public Vector3 Position { get; set; }

        public float Rotation { get; set; }

        public float Scale { get; set; }
    }
}
