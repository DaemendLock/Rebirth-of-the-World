using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public readonly struct HealthValueContainer
    {
        private readonly int _index;
        private readonly HealthValue[] _values;

        public HealthValueContainer(int index, HealthValue[] values)
        {
            _index = index;
            _values = values;
        }

        public ref HealthValue Value => ref _values[_index];
    }

    public readonly ref struct Health
    {
        private readonly HealthValueContainer _healthValueContainer;
        private readonly float _bonusHealth;

        public Health(UnitId id, HealthValueContainer healthValueContainer) : this(id, healthValueContainer, healthValueContainer.Value.Default)
        { }

        public Health(UnitId id, HealthValueContainer healthValueContainer, float bonusHealth)
        {
            if (float.IsNaN(bonusHealth))
            {
                throw new System.InvalidOperationException($"{nameof(bonusHealth)}: Value can't be NaN");
            }

            Id = id;
            _healthValueContainer = healthValueContainer;
            _bonusHealth = bonusHealth;
        }

        public UnitId Id { get; }

        public float DefaultHealth
        {
            readonly get => _healthValueContainer.Value.Default;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new System.InvalidOperationException($"Value can't be NaN");
                }

                _healthValueContainer.Value.Default = value;
            }
        }

        public float CurrentHealth
        {
            readonly get => _healthValueContainer.Value.Current;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new System.InvalidOperationException($"Value can't be NaN");
                }

                ref HealthValue healthValue = ref _healthValueContainer.Value;

                if (value < 0)
                {
                    healthValue.Current = 1;
                    return;
                }

                if (value > MaxHealth)
                {
                    healthValue.Current = MaxHealth;
                }

                healthValue.Current = value;
            }
        }

        public readonly float MaxHealth => _bonusHealth + DefaultHealth;

        public void TakeDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            _healthValueContainer.Value.Current -= damage;
        }

        public void TakeHealing(float healing)
        {
            if (healing < 0)
            {
                return;
            }

            ref HealthValue healthValue = ref _healthValueContainer.Value;

            healthValue.Current += healing;

            if (healthValue.Current > MaxHealth)
            {
                healthValue.Current = MaxHealth;
            }
        }
    }
}
