using Client.Combat.View.UI.Elements;
using Client.Combat.View.UI.ResourceBar;
using System;
using UnityEngine;
using View.Combat.UI.Elements;

namespace Client.Combat.View.Units
{
    public class Nameplate : MonoBehaviour
    {
        [SerializeField] private SelectionFrame _selectionFrame;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private ResourceBar _resources;
        [SerializeField] private Bar _castbar;

        [SerializeField] private Color _friendlyColor;
        [SerializeField] private Color _enemyColor;

        private Unit _attachedUnit;

        public Unit AssignedUnit => _attachedUnit;

        public void AssignTo(Unit unit)
        {
            if (_attachedUnit != null)
            {
                return;
            }

            _attachedUnit = unit ?? throw new ArgumentNullException(nameof(unit));
        }

        public void UpdatePosition(Camera camera)
        {
            transform.position = camera.WorldToScreenPoint(_attachedUnit.transform.position + Vector3.up * 2);
        }

        public void InformSelectionChanged(int selelctedId)
        {
            //TODO: 
            //if (_attachedUnit.Team != selection.Team)
            //{
            //    _healthBar.Color = _enemyColor;
            //    return;
            //}

            _healthBar.Color = _friendlyColor;
        }

        public void SetTargeted(bool targeted)
        {
            if (targeted)
            {
                transform.SetAsLastSibling();
                //transform.localScale = new Vector3(_selectionFrame.FrameScale, _selectionFrame.FrameScale, _selectionFrame.FrameScale);
                //_selectionFrame.Selected = true;
                return;
            }

            //transform.localScale = new Vector3(1, 1, 1);
            //_selectionFrame.Selected = false;
        }
    }
}