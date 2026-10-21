using Combat.Common;
using Combat.Common.Primitives;

namespace Combat.Local.Application.DTO
{
    public readonly ref struct HealthChangeRecord
    {
        public HealthChangeRecord(UnitId target, float currentHealth, float maxHealth, float? oldValue = null)
        {
            Target = target;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            OldValue = oldValue;
        }

        public readonly UnitId Target { get; }
        public readonly float CurrentHealth { get; }
        public readonly float MaxHealth { get; }
        public readonly float? OldValue { get; }
    }

    public readonly ref struct ComposureChangeRecord
    {
        public ComposureChangeRecord(UnitId target, float currentComposure, float maxComposure, float? oldValue = null)
        {
            Target = target;
            CurrentComposure = currentComposure;
            MaxComposure = maxComposure;
            OldValue = oldValue;
        }

        public UnitId Target { get; }
        public float CurrentComposure { get; }
        public float MaxComposure { get; }
        public float? OldValue { get; }
    }

    public readonly ref struct KillRecord
    {
        public readonly UnitId Victim;

        public KillRecord(UnitId victim)
        {
            Victim = victim;
        }
    }

    public readonly ref struct ConsciousStateChangeRecord
    {
        public UnitId Target { get; }
        public ConsciousState State { get; }
    }
}
