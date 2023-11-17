using Core.Combat.Interfaces;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using System.Runtime.CompilerServices;
using UnityEngine;
using View.Combat.UI.Elements;

namespace View.Combat.UI.ResourceBar
{
    public class ResourceBar : MonoBehaviour, UnitAssignable
    {
        [SerializeField] private Bar _leftBar;
        [SerializeField] private Bar _rightBar;

        private Unit _unit;

        private Resource _left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _leftBar.MaxValue = value.MaxValue;
                _leftBar.Value = value.Value;
            }
        }

        private Resource _right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _rightBar.MaxValue = value.MaxValue;
                _rightBar.Value = value.Value;
            }
        }

        private void Update()
        {
            if(_unit == null)
            {
                return;
            }

            (_left, _right) = _unit.GetResources();
        }

        public void AssignTo(Unit selection)
        {
            _unit = selection;

            (ResourceType left, ResourceType right) = _unit.GetResourceTypes();
            _leftBar.SetResourceType(left);
            _rightBar.SetResourceType(right);
        }
    }
}
