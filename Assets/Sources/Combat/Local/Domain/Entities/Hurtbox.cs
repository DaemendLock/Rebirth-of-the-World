using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public struct Hurtbox
    {
        public Hurtbox(HurtboxId id, HurtboxType type, EntityId ownerId)
        {
            Id = id;
            Owner = ownerId;
            Type = type;
        }

        public HurtboxId Id { get; }

        public HurtboxType Type { get; }

        public EntityId Owner { get; }
    }
}
