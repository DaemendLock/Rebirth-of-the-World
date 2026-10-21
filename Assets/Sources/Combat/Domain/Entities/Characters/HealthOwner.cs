using Combat.Common.Primitives;

namespace Combat.Domain.Entities
{
    public struct HealthOwner
    {
        private float _currentValue;
        private float _bonusHealth;
        private float _currentComposure;
        private float _bonusComposure;

        public HealthOwner(UnitId id, float currentHealth, float defaultHealth) : this(id, currentHealth, defaultHealth, 0f)
        { }

        public HealthOwner(UnitId id, float currentHealth, float defaultHealth, float bonusHealth)
            : this(id, currentHealth, defaultHealth, 100f, 100f, bonusHealth)
        { }

        public HealthOwner(UnitId id, float currentHealth, float defaultHealth,
            float currentComposure, float defaultComposure, float bonusHealth = 0f, float bonusComposure = 0f)
        {
            if (float.IsNaN(bonusHealth))
            {
                throw new System.InvalidOperationException($"{nameof(bonusHealth)}: Value can't be NaN");
            }

            if (float.IsNaN(defaultComposure) || defaultComposure < 0f)
            {
                throw new System.InvalidOperationException("Default composure must be non-negative and can't be NaN");
            }

            Id = id;
            _currentValue = currentHealth;
            Default = defaultHealth;
            _bonusHealth = bonusHealth;
            _currentComposure = 0f;
            _bonusComposure = 0f;
            DefaultComposure = defaultComposure;
            BonusComposure = bonusComposure;
            CurrentComposure = currentComposure;
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

        public float DefaultComposure { get; set; }

        public float CurrentComposure
        {
            readonly get => _currentComposure;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new System.InvalidOperationException("Value can't be NaN");
                }

                _currentComposure = System.Math.Max(0f, System.Math.Min(value, MaxComposure));
            }
        }

        public float BonusComposure
        {
            readonly get => _bonusComposure;
            set
            {
                if (float.IsNaN(value) || value < 0f)
                {
                    throw new System.InvalidOperationException("Bonus composure must be non-negative and can't be NaN");
                }

                _bonusComposure = value;
            }
        }

        public readonly float MaxComposure => _bonusComposure + DefaultComposure;

        public void TakeDamage(float healthDamage, float composureDamage)
        {
            TakeDamage(healthDamage);

            if (composureDamage >= 0f)
            {
                CurrentComposure -= composureDamage;
            }
        }

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
