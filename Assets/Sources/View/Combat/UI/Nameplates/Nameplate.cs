using UnityEngine;
using View.Combat.UI.Elements;
using View.Combat.UI.Elemets;
using View.Combat.UI.ResourceBar;

namespace View.Combat.Units
{
    public class Nameplate : MonoBehaviour
    {
        [SerializeField] private SelectionFrame _selectionFrame;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private ResourceBar _resources;
        [SerializeField] private Bar _castbar;

        [SerializeField] private Color _friendlyColor;
        [SerializeField] private Color _enemyColor;

        private Core.Combat.Units.Unit _valueSource;

        public int AssignedId => _valueSource != null ? _valueSource.Id : -1;

        public void AssignTo(int id)
        {
            if (_valueSource != null)
            {
                return;
            }

            _valueSource = Core.Combat.Engine.Units.GetUnit(id);

            if(_valueSource == null)
            {
                return;
            }

            _healthBar.AssignTo(_valueSource);
        }

        public void UpdatePostiotn(UnityEngine.Camera camera)
        {
            Utils.DataTypes.Vector3 position = _valueSource.Position;
            transform.position = camera.WorldToScreenPoint(new Vector3(position.x, position.y, position.z) + Vector3.up * 2);
        }

        public void InformSelectionChanged(int selelctedId)
        {
            Core.Combat.Units.Unit selection = Core.Combat.Engine.Units.GetUnit(selelctedId);

            if (selection == null)
            {
                return;
            }

            if (_valueSource.Team != selection.Team)
            {
                _healthBar.Color = _enemyColor;
                return;
            }

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