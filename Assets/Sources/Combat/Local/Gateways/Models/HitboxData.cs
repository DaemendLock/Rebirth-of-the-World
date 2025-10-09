using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Data.Models
{
    public readonly struct HitData
    {
        public HitData(HitboxId hitboxId, HurtboxId hurtboxId, Vector3 location)
        {
            HitboxId = hitboxId;
            HurtboxId = hurtboxId;
            Location = location;
        }

        public HitboxId HitboxId { get; }
        public HurtboxId HurtboxId { get; }
        public Vector3 Location { get; }
    }

    public readonly struct HitboxData
    {
        public HitboxData(HitboxType type, EntityId owner)
        {
            Type = type;
            Owner = owner;
        }

        public HitboxType Type { get; }

        public EntityId Owner { get; }
    }

    public readonly struct HurtboxData
    {
        public HurtboxData(HurtboxType type, EntityId owner)
        {
            Type = type;
            Owner = owner;
        }

        public HurtboxType Type { get; }

        public EntityId Owner { get; }
    }
}
