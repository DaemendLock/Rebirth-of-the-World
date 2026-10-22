using Combat.Common.Primitives;

namespace Combat.Domain.Entities
{

    public struct HealthOwner
    {
        private float _currentValue;
        private float _bonusHealth;

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

        public float Default { get; set; }

        public float CurrentHealth
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

        public float BonusHealth
        {
            readonly get => _bonusHealth;
            set
            {
                if (value < 0)
                {
                    throw new System.InvalidOperationException();
                }

                _bonusHealth = value;
            }
        }

        public readonly float MaxHealth => _bonusHealth + Default;

        public void TakeDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            CurrentHealth -= damage;
        }

        public void TakeHealing(float healing)
        {
            if (healing < 0)
            {
                return;
            }

            CurrentHealth += healing;
        }
    }
}
