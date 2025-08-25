using System.Collections;

using Client.Combat.Infrastructure.Controllers;
using Client.Combat.Presentation.Implementations.Units;

using Server.Combat.Networking;

using UnityEngine;

using Zenject;

namespace Assets.Sources.Temp
{
    public readonly struct UserInput
    {
        public readonly Vector2 MoveDirection;
        public readonly Vector2 MouseMovement;
        public readonly ActionInputs Actions;

        public UserInput(Vector2 moveDirection, Vector2 mouseMovement, ActionInputs actions)
        {
            MoveDirection = moveDirection;
            MouseMovement = mouseMovement;
            Actions = actions;
        }
    }

    public interface IInputReader
    {
        UserInput ReadInput();
    }

    public class KeyboardInputReader : IInputReader
    {
        private readonly BitArray _castInputs = new(16);

        private bool _lockCursor;

        public KeyboardInputReader()
        {
        }

        public UserInput ReadInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (_lockCursor)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }

                _lockCursor = !_lockCursor;
            }

            Vector2 mouseDelta = _lockCursor ? GetMouseMovement() : Vector2.zero;

            Vector3 moveDirection = GetMoveDirection();
            ActionInputs actions = GetCastInputs();
            UserInput result = new(moveDirection, mouseDelta, actions);
            return result;
        }

        private static Vector2 GetMouseMovement() => new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        private static Vector3 GetMoveDirection()
        {
            Vector2 direction2d = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                direction2d += Vector2.up;
            }

            if (Input.GetKey(KeyCode.S))
            {
                direction2d += Vector2.down;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                direction2d += Vector2.left;
            }

            if (Input.GetKey(KeyCode.E))
            {
                direction2d += Vector2.right;
            }

            return direction2d;
        }

        private ActionInputs GetCastInputs()
        {
            _castInputs[0] = Input.GetKeyDown(KeyCode.Mouse1);
            _castInputs[1] = Input.GetKeyDown(KeyCode.Alpha1);
            _castInputs[2] = Input.GetKeyDown(KeyCode.Alpha2);
            _castInputs[9] = Input.GetKeyDown(KeyCode.Space);

            int result = 0;

            for (int i = 0; i < _castInputs.Length; i++)
            {
                result |= (_castInputs[i] ? 1 : 0) << i;
            }

            return new(result);
        }
    }

    [RequireComponent(typeof(UnitPresenter))]
    public class UnitViewInputReaderCompenent : MonoBehaviour
    {
        [Inject] private ClientInputReader _clientInputReader;
        [Inject] private ICameraController _cameraController;

        private IInputReader _inputReader;
        private UnitPresenter _unitView;

        private void Awake()
        {
            _inputReader = new KeyboardInputReader();
            _unitView = GetComponent<UnitPresenter>();
        }

        private void Update()
        {
            if (_unitView.Model == null)
            {
                return;
            }

            UserInput playerInput = _inputReader.ReadInput();

            int id = _unitView.Model.Id;
            Vector3 position = _unitView.transform.position;
            float rotation = _unitView.transform.rotation.eulerAngles.y + playerInput.MouseMovement.x;
            Vector3 moveDirection = Quaternion.AngleAxis(rotation, Vector3.up) * new Vector3(playerInput.MoveDirection.x, 0, playerInput.MoveDirection.y);

            InputData inputData = new(new(id), position, rotation, moveDirection, playerInput.Actions);
            _cameraController.Rotate(playerInput.MouseMovement);

            if (_unitView.Model.CanMove())
            _unitView.transform.rotation *= Quaternion.AngleAxis(playerInput.MouseMovement.x, Vector3.up);

            _clientInputReader.AddInputData(inputData);
        }
    }
}
