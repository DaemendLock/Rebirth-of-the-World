using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly struct ScaleOverTimeEffect
    {
        public EntityId Target { get; }

        public float Rate { get; }
    }
}
