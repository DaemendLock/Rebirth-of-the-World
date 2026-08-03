using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public interface IHealth
    {
        float CurrentHealth { get; set; }
        float MaxHealth { get; }
        float DefaultHealth { get; set; }
    }

    public readonly ref struct HealthValueContainer
    {
        private readonly HealthValue[] _values;
        private readonly int _index;

        public HealthValueContainer(int index, HealthValue[] values)
        {
            _index = index;
            _values = values;
        }

        public ref HealthValue Value => ref _values[_index];
    }

    public ref struct Health
    {
        private HealthValueContainer _healthValueContainer;
        private readonly float _bonusHealth;

        public Health(UnitId id, HealthValueContainer healthValueContainer) : this(id, healthValueContainer, 0f)
        { }

        public Health(UnitId id, HealthValueContainer healthValueContainer, float bonusHealth)
        {
            if (float.IsNaN(bonusHealth))
            {
                throw new System.InvalidOperationException($"{nameof(bonusHealth)}: Value can't be NaN");
            }

            _healthValueContainer = healthValueContainer;
            _bonusHealth = bonusHealth;
            Id = id;
        }

        public UnitId Id { get; }

        public readonly float DefaultHealth
        {
            get => _healthValueContainer.Value.Default;
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

            ref HealthValue healthValue = ref _healthValueContainer.Value;
            healthValue.Current -= damage;
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
