using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly ref struct HitRecord
    {
        public HitRecord(HitboxId hitboxId, HurtboxId hurtboxId, Vector3 location)
        {
            HitboxId = hitboxId;
            HurtboxId = hurtboxId;
            Location = location;
        }

        public HitboxId HitboxId { get; }

        public HurtboxId HurtboxId { get; }

        public Vector3 Location { get; }
    }

    public readonly ref struct Hitbox
    {
        public Hitbox(HitboxId hitterId, HitboxType type, EntityId owner)
        {
            Id = hitterId;
            Type = type;
            Owner = owner;
        }

        public HitboxId Id { get; }

        public HitboxType Type { get; }

        public EntityId Owner { get; }
    }
}
