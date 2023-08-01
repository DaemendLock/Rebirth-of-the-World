using Remaster.Interfaces;
using Remaster.UnitComponents;
using UnityEngine;

namespace Remaster.View
{
    public class UnitView : MonoBehaviour, UnitAssignable
    {
        private Unit _unit;
        private PositionComponent _position;

        private void Update()
        {
            _position.EvaluateNextLocation(_unit.GetStat(Stats.UnitStat.SPEED), Time.deltaTime);

            transform.position =  _position.Position.Location;
            LookInDirection(_position.Position.ViewDirection);
        }

        public bool TryAssignTo(Unit unit)
        {
            if (_unit != null)
            {
                return false;
            }

            _unit = unit;
            return true;
        }

        private void LookInDirection(Vector3 direction)
        {

        }
    }
}
