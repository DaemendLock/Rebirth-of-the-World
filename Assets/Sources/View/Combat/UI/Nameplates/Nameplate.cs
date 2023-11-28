using UnityEngine;
using View.Combat.UI.Elements;
using View.Combat.UI.Nameplates.Elemets;
using View.Combat.UI.ResourceBar;

namespace View.Combat.Units
{
    public class Nameplate : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private ResourceBar _resources;
        [SerializeField] private Bar _castbar;

        private Core.Combat.Units.Unit _valueSource;

        public int AssignedId => _valueSource != null ? _valueSource.Id : -1;

        public void AssignTo(int id)
        {
            if (_valueSource != null)
            {
                return;
            }

            _valueSource = Core.Combat.Engine.Combat.GetUnit(id);
        }

        public void UpdatePostiotn(UnityEngine.Camera camera)
        {
            Utils.DataTypes.Vector3 position = _valueSource.Position;
            transform.position = camera.WorldToScreenPoint(new Vector3(position.x, position.y, position.z) + Vector3.up * 2);
        }

        public void SetSellected(bool selelcted)
        {

        }
    }
}