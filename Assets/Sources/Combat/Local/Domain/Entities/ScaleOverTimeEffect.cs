using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Units
{
    public readonly struct ScaleOverTimeEffect
    {
        public UnitId Target { get; }

        public float Rate { get; }
    }
}
