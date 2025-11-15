using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public ref struct Health
    {
        private readonly float _maxHealth;
        private float _currentHealth;
        private float _defaultHealth;

        public Health(EntityId id, float defaultHealth) : this(id, 0, defaultHealth, defaultHealth)
        { }

        public Health(EntityId id, float currentHealth, float maxHealth, float defaultHealth)
        {
            if (float.IsNaN(currentHealth))
            {
                throw new System.InvalidOperationException($"{nameof(currentHealth)}: Value can't be NaN");
            }

            if (float.IsNaN(maxHealth))
            {
                throw new System.InvalidOperationException($"{nameof(maxHealth)}: Value can't be NaN");
            }

            if (float.IsNaN(defaultHealth))
            {
                throw new System.InvalidOperationException($"{nameof(defaultHealth)}: Value can't be NaN");
            }

            Id = id;
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
            _defaultHealth = defaultHealth;
        }

        public EntityId Id { get; }

        public float DefaultHealth
        {
            get => _defaultHealth;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new System.InvalidOperationException($"Value can't be NaN");
                }

                _defaultHealth = value;
            }
        }

        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new System.InvalidOperationException($"Value can't be NaN");
                }

                if (value < 0)
                {
                    _currentHealth = 1;
                    return;
                }

                if (value > MaxHealth)
                {
                    _currentHealth = MaxHealth;
                }

                _currentHealth = value;
            }
        }

        public float MaxHealth => _maxHealth;

        public void TakeDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            _currentHealth -= damage;
        }

        public void TakeHealing(float healing)
        {
            if (healing < 0)
            {
                return;
            }

            _currentHealth += healing;

            if (_currentHealth > MaxHealth)
            {
                _currentHealth = MaxHealth;
            }
        }
    }
}
