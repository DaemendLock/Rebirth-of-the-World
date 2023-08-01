using Remaster.Interfaces;
using Remaster.View.UIElements;
using UnityEngine;

namespace Remaster.View
{
    public class Nameplate : MonoBehaviour, UnitAssignable
    {
        [SerializeField] private Healthbar _healthbar;

        private Unit _unit;

        public bool TryAssignTo(Unit unit)
        {
            if (_unit != null)
            {
                return false;
            }

            _unit = unit;
            enabled = true;
            return true;
        }

        private void OnEnable()
        {
            _unit.OnHealthChanged += _healthbar.OnHealthChanged;
        }

        private void OnDisable()
        {
            if (_unit == null)
            {
                return;
            }

            _unit.OnHealthChanged -= _healthbar.OnHealthChanged;
        }
    }
}
