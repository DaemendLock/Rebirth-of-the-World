using Combat.Common.Primitives;

namespace Combat.Domain.Entities
{
    public struct ComposureOwner
    {
        public const float RegenRate = 0.10f;
        public const float BaseDrainRate = 0.05f;
        public const float HealingDelay = 3f;

        private float _currentValue;
        private float _bonus;
        private float _healingCooldown;

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
            Default = defaultHealth;
            _bonus = bonus;
            _healingCooldown = 0f;
            BaseDrainModifier = 1f;
        }

        public UnitId Id { get; }

        public float Default { get; set; }

        public float Current
        {
            readonly get => _currentValue;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new System.InvalidOperationException("Value can't be NaN");
                }

                if (value < 0f)
                {
                    _currentValue = 0;
                    return;
                }

                _currentValue = System.Math.Min(value, Max);
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

        public readonly float Max => _bonus + Default;

        public float HealingCooldown
        {
            readonly get => _healingCooldown;
            set => _healingCooldown = System.Math.Max(value, 0);
        }

        public float BaseDrainModifier { get; set; }

        public void TakeDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            Current -= damage;
        }

        public void TakeHealing(float healing)
        {
            if (healing < 0)
            {
                return;
            }

            if (Current > Max)
            {
                return;
            }

            Current += healing;
        }

        public void Progress(float deltaTime) => HealingCooldown -= deltaTime;
    }
}
