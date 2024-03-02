using Client.Combat.Core.Units;
using Client.Combat.View.UI.Panels;
using UnityEngine;
using View.Combat.Camera;

namespace Client.Combat.View.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private ActionBar.ActionBar _actionBar;
        [SerializeField] private ResourceBar.ResourceBar _resourceBar;
        [SerializeField] private UnitPanel _unitPanel;
        [SerializeField] private UnitPanel _targetPanel;
        [SerializeField] private CameraController _cameraController;

        private Unit _selection;
        private Unit _target;

        public static UIRoot Instance { get; private set; }

        public float CameraDistance
        {
            get => _cameraController.CameraDistance;
            set => _cameraController.SetDistance(value);
        }

        public Quaternion CameraRotation
        {
            set => _cameraController.CameraRotation = value;
        }

        private void Start()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                throw new System.InvalidOperationException($"Instance of {typeof(UIRoot)} already exists.");
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void DisplayUnit(Unit unit)
        {
            if (unit == null)
            {
                return;
            }

            //TODO: _cameraController.AssignTo(unit);

            //TODO: _unitPanel.AssignTo(_selection);
        }

        public void DisplayTarget(Unit unit)
        {
            _target = unit;

            _targetPanel.UpdateHealth(unit.Health);
            _targetPanel.ShowCast(unit.SpellCasting.ActiveCast);
        }
    }
}
