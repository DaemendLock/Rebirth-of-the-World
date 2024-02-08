using Core.Combat.Interfaces;
using Core.Combat.Units;
using UnityEngine;
using View.Combat.UI.Elements;

namespace View.Combat.UI.Elemets
{
    public class HealthBar : MonoBehaviour, UnitAssignable
    {
        [SerializeField] private Bar _health;
        [SerializeField] private Bar _absorption;

        private Unit _unit;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public Color Color
        {
            get => _health.Color;
            set => _health.Color = value;
        }

        private void Update()
        {
            if (_unit == null)
            {
                return;
            }

            _health.MaxValue = _unit.MaxHealth;
            _health.Value = _unit.CurrentHealth;
            _absorption.MaxValue = _unit.MaxHealth;
            _absorption.Value = _unit.GetAbsorption();
        }

        public void AssignTo(Unit unit)
        {
            if (unit == null)
            {
                gameObject.SetActive(false);
                _unit = null;
                return;
            }

            if (_unit == null)
            {
                gameObject.SetActive(true);
            }

            _unit = unit;
        }
    }
}
