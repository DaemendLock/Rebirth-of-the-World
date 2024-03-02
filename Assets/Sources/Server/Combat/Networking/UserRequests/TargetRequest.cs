using Core.Combat.Engine;
using Core.Combat.Units;

namespace Server.Combat.UserRequests
{
    internal class TargetRequest : InputRequest
    {
        private readonly Unit _attacker;
        private readonly Unit _target;

        public TargetRequest(int attackerId, int targetId)
        {
            _attacker = Units.GetUnitById(attackerId);
            _target = Units.GetUnitById(targetId);
        }

        public bool IsValid() => _attacker.Team != _target.Team;

        public void Perform(UpdateRecord target)
        {
            Units.StartAttack(_attacker, _target);
        }
    }
}
