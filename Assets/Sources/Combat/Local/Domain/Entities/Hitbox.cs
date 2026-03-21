using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
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
