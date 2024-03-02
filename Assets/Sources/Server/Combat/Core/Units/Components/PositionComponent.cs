using Core.Combat.Engine;
using Core.Combat.Utils.MovementControllers;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public sealed class PositionComponent
    {
        private readonly Unit _unit;
        private UnitSpeedMovementController _moveController;

        public PositionComponent(Unit unit)
        {
            _unit = unit;
            _moveController = new UnitSpeedMovementController(unit);
        }

        public Vector3 MoveDirection { get => _moveController.MoveDirection; set => _moveController.MoveDirection = value; }

        public bool IsMoving => MoveDirection != Vector3.zero;

        public Vector3 GetNextPosition(Vector3 currentPosition, float speed)
        {
            if (IsMoving == false)
            {
                return currentPosition;
            }

            return currentPosition + MoveDirection * ((float) (speed * ModelUpdate.UpdateTime) / 1000);
        }
    }
}
