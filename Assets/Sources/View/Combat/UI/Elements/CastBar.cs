using Core.Combat.Abilities;
using Core.Combat.Interfaces;
using Core.Combat.Units;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace View.Combat.UI.Elements
{
    public class CastBar : MonoBehaviour, UnitAssignable
    {
        [SerializeField] private Bar _bar;

        private Unit _unit;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_unit == null)
            {
                return;
            }

            _unit.StartedPrecast -= ShowCast;
            _unit.StoppedCast -= HideCast;
        }

        private void Update()
        {
            _bar.Value = _unit.CastTime;
        }

        public void AssignTo(Unit unit)
        {
            OnDestroy();

            _unit = unit;
            Subsribe();
        }

        private void ShowCast(Spell spell, float duration)
        {
            if (duration == 0)
            {
                return;
            }

            gameObject.SetActive(true);
            _bar.MaxValue = duration;
        }

        private void HideCast()
        {
            gameObject.SetActive(false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Subsribe()
        {
            if (_unit == null)
            {
                return;
            }

            _unit.StartedPrecast += ShowCast;
            _unit.StoppedCast += HideCast;
        }
    }
}
