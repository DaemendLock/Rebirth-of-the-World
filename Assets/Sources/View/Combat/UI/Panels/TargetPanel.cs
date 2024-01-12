using Core.Combat.Interfaces;
using Core.Combat.Units;
using UnityEngine;
using View.Combat.UI.Elements;
using View.Combat.UI.Elemets;

namespace View.Combat.UI.Panels
{
    internal class TargetPanel : MonoBehaviour, UnitAssignable
    {
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CastBar _castBar;

        private Unit _assignedUnit;

        public void AssignTo(Unit unit)
        {
            if(unit == null)
            {
                _assignedUnit = null;
                gameObject.SetActive(false);
                return;
            }
            
            if(_assignedUnit == null)
            {
                gameObject.SetActive(true);
            }

            _assignedUnit = unit;

            _healthBar.AssignTo(unit);
            _castBar.AssignTo(unit);
        }
    }
}
