using Core.Combat.Engine;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public class PositionComponent
    {
        public Vector3 MoveDirection { get; set; } = Vector3.zero;

        public bool IsMoving => MoveDirection != Vector3.zero;

        public Vector3 GetNextPosition(Vector3 currentPosition, float speed)
        {
            if (IsMoving == false)
            {
                return currentPosition;
            }

            return currentPosition + (MoveDirection * (speed * ModelUpdate.UpdateTime / 1000));
        }
    }
}
