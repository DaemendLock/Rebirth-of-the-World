using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Core.Combat.Units;

namespace Server.Combat.UserRequests
{
    internal class StopRequest : InputRequest
    {
        private Unit _unit;

        public StopRequest(int id)
        {
            _unit = Units.GetUnitById(id);
        }

        public bool IsValid() => true;

        public void Perform(UpdateRecord target)
        {
            Units.StopAllActions(_unit);
        }
    }
}
