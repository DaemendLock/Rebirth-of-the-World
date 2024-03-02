using Core.Combat.Engine;
using Core.Combat.Units;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Utils.MovementControllers
{
    internal interface MovementController
    {
        Vector3 GetMovement(long deltaTime);
    }

    internal class UnitSpeedMovementController : MovementController
    {
        private Unit _speedOwner;

        public UnitSpeedMovementController(Unit speedOwner)
        {
            _speedOwner = speedOwner;
        }

        public Vector3 MoveDirection { get; set; } = Vector3.zero;

        public Vector3 GetMovement(long deltaTime)
        {
            if (MoveDirection == Vector3.zero)
            {
                return Vector3.zero;
            }

            return MoveDirection * ((float) (StatsEvaluator.EvaluateUnitStatValue(UnitStat.SPEED, _speedOwner) * ModelUpdate.UpdateTime) / 1000.0f);
        }
    }
}