using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly ref struct Hurtbox
    {
        public Hurtbox(HurtboxId id, HurtboxType type, EntityId owner)
        {
            Id = id;
            Type = type;
            Owner = owner;
        }

        public HurtboxId Id { get; }

        public HurtboxType Type { get; }

        public EntityId Owner { get; }
    }
}
