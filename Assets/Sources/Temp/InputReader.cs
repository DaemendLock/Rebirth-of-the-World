using Client.Combat.Input.Handlers;
using Client.Combat.Input.Selection;

using Input;

using UnityEngine;

namespace Temp
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private bool _useRTSControl = false;

        private CombatInput _input;
        private CastInputHandler _castHandler;
        private MovementInputHandler _movementHandler;
        private TargetSelectionHandler _selectionHandler;

        private SelectionInfo _selection;

        private void Awake()
        {
            if (_useRTSControl)
            {

            }

            _selection = new SelectionInfo();

            _input = new();
            _castHandler = new(_input, _selection);
            _movementHandler = new(_input);
            _selectionHandler = new(_input);
        }

        private void OnEnable()
        {
            _castHandler.Enable();
            _movementHandler.Enable();
            _selectionHandler.Enable();
        }

        private void OnDisable()
        {
            _castHandler.Disable();
            _movementHandler.Disable();
            _selectionHandler.Disable();
        }
    }
}
