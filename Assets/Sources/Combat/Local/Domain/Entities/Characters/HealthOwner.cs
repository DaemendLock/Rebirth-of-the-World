using Combat.Common.Primitives;

namespace Combat.Local.Domain.Entities
{
    public ref struct HealthOwner
    {
        private readonly float _bonusHealth;
        private float _currentValue;

        public HealthOwner(UnitId id, float currentHealth, float defaultHealth) : this(id, currentHealth, defaultHealth, 0f)
        { }

        public HealthOwner(UnitId id, float currentHealth, float defaultHealth, float bonusHealth)
        {
            if (float.IsNaN(bonusHealth))
            {
                throw new System.InvalidOperationException($"{nameof(bonusHealth)}: Value can't be NaN");
            }

            Id = id;
            _currentValue = currentHealth;
            Default = defaultHealth;
            _bonusHealth = bonusHealth;
        }

        public UnitId Id { get; }

        public readonly float Default { get; }

        public float CurrentValue
        {
            readonly get => _currentValue;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new System.InvalidOperationException($"Value can't be NaN");
                }

                if (value > MaxHealth)
                {
                    _currentValue = MaxHealth;
                    return;
                }

                _currentValue = value;
            }
        }

        public readonly float MaxHealth => _bonusHealth + Default;

        public void TakeDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            CurrentValue -= damage;
        }

        public void TakeHealing(float healing)
        {
            if (healing < 0)
            {
                return;
            }

            CurrentValue += healing;
        }
    }
}
