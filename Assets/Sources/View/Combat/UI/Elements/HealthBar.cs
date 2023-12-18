using Core.Combat.Interfaces;
using Core.Combat.Units;
using UnityEngine;
using View.Combat.UI.Elements;

namespace View.Combat.UI.Nameplates.Elemets
{
    public class HealthBar : MonoBehaviour, UnitAssignable
    {
        [SerializeField] private Bar _health;
        [SerializeField] private Bar _absorption;

        private Unit _unit;

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
            _unit = unit;
        }

        public void SetColor(Color color)
        {
            _health.Color = color;
        }
    }
}
