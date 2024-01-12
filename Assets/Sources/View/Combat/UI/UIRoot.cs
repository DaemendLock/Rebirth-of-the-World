using Core.Combat.Interfaces;
using Core.Combat.Units;
using UnityEngine;
using View.Combat.Camera;
using View.Combat.UI.Panels;

namespace View.Combat.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private ActionBar.ActionBar _actionBar;
        [SerializeField] private ResourceBar.ResourceBar _resourceBar;
        [SerializeField] private UnitPanel _unitPanel;
        [SerializeField] private TargetPanel _targetPanel;
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
                return;
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

        public void DisplayUnit(int id)
        {
            _selection = Core.Combat.Engine.Combat.GetUnit(id);

            if (_selection == null)
            {
                return;
            }

            _actionBar.AssignTo(_selection);
            _unitPanel.AssignTo(_selection);
            _resourceBar.AssignTo(_selection);
            _cameraController.AssignTo(_selection);
        }
        public void DisplayTarget(int id)
        {
            _target = Core.Combat.Engine.Combat.GetUnit(id);

            _targetPanel.AssignTo(_target);
        }
    }
}
