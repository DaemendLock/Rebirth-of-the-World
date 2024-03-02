using Client.Combat.Adapters;
using Client.Combat.Input.Selection;

using Input;

using System;

using UnityEngine;

namespace Client.Combat.Input.Handlers
{
    public class MovementInputHandler
    {
        private CombatInput.Combat3rdPersonActions _moveAction3rd;
        private CombatInput.CombatRTSActions _moveActionRTS;

        private readonly SelectionInfo _selection;

        private Vector2 _moveDirection;
        private Vector2 _cameraRotation;
        private Vector3 _mousePosition;

        public MovementInputHandler(CombatInput source)
        {
            _moveAction3rd = source.Combat3rdPerson;
            _moveActionRTS = source.CombatRTS;
        }

        public static Vector2 CameraAngleDelta { get; private set; }

        public void Enable()
        {
            _moveAction3rd.MoveCharacter.performed += ctx => ModifyMovement(ctx.ReadValue<Vector2>());
            _moveAction3rd.MoveCharacter.canceled += ctx => ModifyMovement(Vector2.zero);
            //TODO: SelectionInfo.SelectionChanged += () => ModifyMovement(Vector2.zero);

            _moveAction3rd.MoveCamera.started += ctx => ChangeCameraAngle(ctx.ReadValue<Vector2>());
            _moveAction3rd.MoveCamera.canceled += ctx => ChangeCameraAngle(Vector2.zero);

            _moveAction3rd.LockCursor.performed += ctx => SetCursorLocked(true);
            _moveAction3rd.LockCursor.canceled += ctx => SetCursorLocked(false);

            _moveAction3rd.DistantCamera.performed += ctx => ChangeCameraDistance(ctx.ReadValue<Vector2>().y);

            _moveAction3rd.Enable();
        }

        public void Disable()
        {
            _moveAction3rd.Disable();

            _moveAction3rd.MoveCharacter.performed -= ctx => ModifyMovement(ctx.ReadValue<Vector2>());
            _moveAction3rd.MoveCharacter.canceled -= ctx => ModifyMovement(Vector2.zero);
            //TODO: SelectionInfo.SelectionChanged -= () => ModifyMovement(Vector2.zero);

            _moveAction3rd.MoveCamera.started -= ctx => ChangeCameraAngle(ctx.ReadValue<Vector2>());
            _moveAction3rd.MoveCamera.canceled -= ctx => ChangeCameraAngle(Vector2.zero);

            _moveAction3rd.LockCursor.performed -= ctx => SetCursorLocked(true);
            _moveAction3rd.LockCursor.canceled -= ctx => SetCursorLocked(false);

            _moveAction3rd.DistantCamera.performed -= ctx => ChangeCameraDistance(ctx.ReadValue<float>());
        }

        private void ModifyMovement(Vector2 direction)
        {
            _moveDirection = direction;
            SendMoveAction();
        }

        private void ChangeCameraAngle(Vector2 delta)
        {
            _cameraRotation += delta;
            _cameraRotation.x %= 360;
            _cameraRotation.y = Math.Clamp(_cameraRotation.y, -90, 90);

            //TODO: UserInputAdapter.HandleCameraAngleChange(_cameraRotation);

            if (_moveDirection != Vector2.zero)
            {
                SendMoveAction();
            }
        }

        private void ChangeCameraDistance(float delta)
        {
            //TODO: UserInputAdapter.ChangeCameraDistance(delta);
        }

        private void SendMoveAction()
        {
            const float DegreeToRadiansDivier = 180 / MathF.PI;

            Utils.DataTypes.Vector3 currentPosition = Utils.DataTypes.Vector3.zero;
            //TODO: Core.Combat.Engine.Units.GetPosition(SelectionInfo.SelectionId).Location;

            float xWorldDirection = (_moveDirection.x * MathF.Cos(_cameraRotation.x / DegreeToRadiansDivier)) + (_moveDirection.y * MathF.Sin(_cameraRotation.x / DegreeToRadiansDivier));
            float yWorldDirection = (-_moveDirection.x * MathF.Sin(_cameraRotation.x / DegreeToRadiansDivier)) + (_moveDirection.y * MathF.Cos(_cameraRotation.x / DegreeToRadiansDivier));

            MoveData data = new(_selection.CurrentTarget, currentPosition, new(xWorldDirection, 0, yWorldDirection), _cameraRotation.x);

            Networking.Combat.Send(data.GetBytes());
            //TODO Core.Combat.Engine.Units.MoveUnit(data.UnitId, data.Position, data.MoveDirection);
        }

        private void SetCursorLocked(bool @lock)
        {
            if (@lock)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
