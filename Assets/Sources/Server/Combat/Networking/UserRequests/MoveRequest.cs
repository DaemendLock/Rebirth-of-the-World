using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Utils.DataTypes;

namespace Server.Combat.UserRequests
{
    internal class MoveRequest : InputRequest
    {
        private readonly int _unitId;
        private readonly Vector3 _direction;

        public MoveRequest(int unit, Vector3 direction)
        {
            _unitId = unit;
            _direction = direction;
        }

        public bool IsValid() => true;

        public IActionRecordContainer Perform()
        {
            Units.MoveUnit(_unitId, _direction);
            return new ActionRecordContainer();
        }

        public void Perform(UpdateRecord target)
        {
            Units.MoveUnit(_unitId, _direction);
        }
    }
}
