using Input;
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
        private bool _isSelected;

        private void OnEnable()
        {
            SellectionInfo.TargetChanged += OnTargetChange;
        }

        private void OnDisable()
        {
            SellectionInfo.TargetChanged -= OnTargetChange;
        }

        public void AssignTo(int id)
        {
            if(_valueSource != null)
            {
                return;
            }

            _valueSource = Core.Combat.Engine.Combat.GetUnit(id);
        }
        
        public void UpdatePostiotn(UnityEngine.Camera camera)
        {
            transform.position = camera.WorldToScreenPoint(Unit.GetUnit(_valueSource.Id).transform.position + Vector3.up * 2);
        }    

        private void OnTargetChange()
        {
            if(_valueSource == null)
            {
                return;
            }

            if ((SellectionInfo.TargetedUnitId != _valueSource.Id) && (_isSelected == false))
            {
                return;
            }
        }
    }
}