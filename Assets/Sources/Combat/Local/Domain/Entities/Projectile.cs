using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Domain.Entities
{
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
