using Combat.Common.Primitives;

namespace Combat.Domain.Entities
{
    public struct ComposureOwner
    {
        private float _currentValue;
        private float _bonus;

        public ComposureOwner(UnitId id, float currentHealth, float defaultHealth) : this(id, currentHealth, defaultHealth, 0f)
        { }

        public ComposureOwner(UnitId id, float currentHealth, float defaultHealth, float bonus)
        {
            if (float.IsNaN(bonus))
            {
                throw new System.InvalidOperationException($"{nameof(bonus)}: Value can't be NaN");
            }

            Id = id;
            _currentValue = currentHealth;
            DefaultComposure = defaultHealth;
            _bonus = bonus;
        }

        public UnitId Id { get; }

        public float CurrentComposure
        {
            readonly get => _currentValue;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new System.InvalidOperationException("Value can't be NaN");
                }

                _currentValue = System.Math.Max(0f, System.Math.Min(value, MaxComposure));
            }
        }

        public float BonusComposure
        {
            readonly get => _bonus;
            set
            {
                if (float.IsNaN(value) || value < 0f)
                {
                    throw new System.InvalidOperationException("Bonus composure must be non-negative and can't be NaN");
                }

                _bonus = value;
            }
        }

        public float DefaultComposure { get; set; }

        public readonly float MaxComposure => _bonus + DefaultComposure;

        public void TakeDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            CurrentComposure -= damage;
        }

        public void TakeHealing(float healing)
        {
            if (healing < 0)
            {
                return;
            }

            CurrentComposure += healing;
        }
    }
}
