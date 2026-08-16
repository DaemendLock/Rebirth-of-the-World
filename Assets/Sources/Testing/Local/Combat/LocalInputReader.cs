using Combat.Common.Primitives;
using Combat.Local.Controllers;

using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

namespace Assets.Sources.Testing.Local
{
    public class LocalInputReader : MonoBehaviour
    {
        [Inject] private readonly PlayerController _playerController;

        [SerializeField] private int _unitId;
        [SerializeField, Min(0.1f)] private float _sensitivity = 1;

        private bool _cameraLock;
        public PlayerId Player { get; private set; }

        private void Start()
        {
            Player = _playerController.Create();
            _playerController.TakeControll(Player, new(_unitId));
        }

        private void Update()
        {
            if (_playerController == null || _playerController.AcceptsInput == false)
            {
                return;
            }

            Vector2 movement = Vector2.zero;

            if (Keyboard.current.wKey.isPressed)
            {
                movement += (Vector2.up);
            }
            if (Keyboard.current.sKey.isPressed)
            {
                movement += (Vector2.down);
            }
            if (Keyboard.current.qKey.isPressed)
            {
                movement += (Vector2.left);
            }
            if (Keyboard.current.eKey.isPressed)
            {
                movement += (Vector2.right);
            }

            _playerController.MoveInDirection(Player, movement);

            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                _playerController.DesireCast(Player, 0);
            }

            if (Keyboard.current.aKey.wasReleasedThisFrame)
            {
                _playerController.ReleaseCast(Player, 0);
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                _playerController.DesireCast(Player, 1);
            }
            if (Keyboard.current.digit1Key.wasReleasedThisFrame)
            {
                _playerController.ReleaseCast(Player, 1);
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                _playerController.DesireCast(Player, 2);
            }
            if (Keyboard.current.digit2Key.wasReleasedThisFrame)
            {
                _playerController.ReleaseCast(Player, 2);
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                _playerController.DesireCast(Player, 3);
            }
            if (Keyboard.current.digit3Key.wasReleasedThisFrame)
            {
                _playerController.ReleaseCast(Player, 3);
            }

            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                _playerController.DesireCast(Player, 4);
            }
            if (Keyboard.current.digit4Key.wasReleasedThisFrame)
            {
                _playerController.ReleaseCast(Player, 4);
            }

            if (_cameraLock)
            {
                Vector2 rotation = Input.mousePositionDelta;
                _playerController.Rotate(Player, rotation * _sensitivity);
            }

            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                LockCamera();
            }

            if (Mouse.current.middleButton.wasReleasedThisFrame)
            {
                ReleaseCamera();
            }
        }

        private void LockCamera()
        {
            if (_cameraLock)
            {
                return;
            }

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            _cameraLock = true;
        }

        private void ReleaseCamera()
        {
            if (_cameraLock == false)
            {
                return;
            }

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            _cameraLock = false;
        }
    }
}
