using Client.Combat.Infrastructure.Input;

using System;

using UnityEngine;

namespace Client.Combat.Input.Handlers
{
    public class MovementInputHandler
    {
        private CombatInput.CombatMovementActions _moveAction3rd;

        private readonly Selection.SelectionInfo _selection;

        private Vector2 _moveDirection;
        private Vector2 _cameraRotation;

        public MovementInputHandler(CombatInput source)
        {
            _moveAction3rd = source.CombatMovement;
        }

        public void Enable()
        {
            _moveAction3rd.MoveCharacter.performed += ctx => ModifyMovement(ctx.ReadValue<Vector2>());
            _moveAction3rd.MoveCharacter.canceled += ctx => ModifyMovement(Vector2.zero);

            _moveAction3rd.LockCursor.performed += ctx => SetCursorLocked(true);
            _moveAction3rd.LockCursor.canceled += ctx => SetCursorLocked(false);

            _moveAction3rd.Enable();
        }

        private void ModifyMovement(Vector2 direction)
        {
            _moveDirection = direction;
            SendMoveAction();
        }

        private void SendMoveAction()
        {
            const float DegreeToRadiansDivier = 180 / MathF.PI;

            Vector3 currentPosition = Vector3.zero;
            //TODO: Core.Combat.Engine.Units.GetPosition(SelectionInfo.SelectionId).Location;

            float xWorldDirection = (_moveDirection.x * MathF.Cos(_cameraRotation.x / DegreeToRadiansDivier)) + (_moveDirection.y * MathF.Sin(_cameraRotation.x / DegreeToRadiansDivier));
            float yWorldDirection = (-_moveDirection.x * MathF.Sin(_cameraRotation.x / DegreeToRadiansDivier)) + (_moveDirection.y * MathF.Cos(_cameraRotation.x / DegreeToRadiansDivier));

            //MoveData data = new(_selection.CurrentTarget, currentPosition, new(xWorldDirection, 0, yWorldDirection), _cameraRotation.x);

            //Networking.Combat.Send(data.GetBytes());
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
