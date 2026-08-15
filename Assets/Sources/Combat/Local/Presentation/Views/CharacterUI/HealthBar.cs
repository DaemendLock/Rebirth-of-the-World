using Combat.View.Common;

using UnityEngine;

namespace Combat.View.CharacterUI
{
    public sealed class HealthBar : MonoBehaviour
    {
        [SerializeField] private Bar _healthBar;
        [SerializeField] private Bar _absorptionBar;

        private float _currentHealth;
        private float _maxHealth;
        private float _absorption;

        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                _healthBar.FillPercent = Mathf.Min(_currentHealth / _maxHealth, 1);
            }
        }
        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                _healthBar.FillPercent = Mathf.Min(_currentHealth / _maxHealth, 1);
            }
        }

        public float Absorption
        {
            get => _absorption;
            set
            {
                _absorption = value;
                _healthBar.FillPercent = Mathf.Min(_absorption / _maxHealth, 1);
            }
        }
    }
}
