using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.ValueObjects
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
}
