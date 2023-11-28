using Input;
using Input.Combat;
using UnityEngine;

namespace Temp
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private bool _useRTSControl = false;

        public static int SelectedUnit = 0;

        private CombatInput _input;
        private CastInputHandler _castHandler;
        private MovementInputHandler _movementHandler;
        private TargetSellectionHandler _sellectionHandler;

        private void Awake()
        {
            if (_useRTSControl)
            {

            }

            _input = new();
            _castHandler = new(_input);
            _movementHandler = new(_input);
            _sellectionHandler = new(_input);
        }

        private void OnEnable()
        {
            _castHandler.Enable();
            _movementHandler.Enable();
            _sellectionHandler.Enable();
        }

        private void OnDisable()
        {
            _castHandler.Disable();
            _movementHandler.Disable();
            _sellectionHandler.Disable();
        }
    }
}
