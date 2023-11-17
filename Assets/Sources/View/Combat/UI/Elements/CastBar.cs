using Core.Combat.Abilities;
using Core.Combat.Interfaces;
using Core.Combat.Units;
using UnityEngine;

namespace View.Combat.UI.Elements
{
    public class CastBar : MonoBehaviour, UnitAssignable
    {
        [SerializeField] private Bar _bar;

        private Unit _unit;

        private void OnEnable()
        {
            if (_unit == null)
            {
                return;
            }

            _unit.StartedPrecast += ShowCast;
            _unit.StopedCast+= HideCast;
        }

        private void OnDisable()
        {
            _unit.StartedPrecast -= ShowCast;
            _unit.StopedCast += HideCast;
        }

        public void AssignTo(Unit unit)
        {
            OnDisable();

            _unit = unit;
            OnEnable();
        }

        private void ShowCast(Spell spell, float duration)
        {
            if(duration == 0)
            {
                return;
            }

            gameObject.SetActive(true);
        }

        private void HideCast()
        {
            gameObject.SetActive(false);
        }
    }
}
