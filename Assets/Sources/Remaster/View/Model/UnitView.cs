using Core.Combat.Interfaces;
using Core.Combat.Stats;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using UnityEngine;

namespace View
{
    public class UnitView : MonoBehaviour, UnitAssignable
    {
        private Unit _unit;
        private PositionComponent _position;

        private void Update()
        {
            _position.EvaluateNextLocation(_unit.GetStat(UnitStat.SPEED), Time.deltaTime);

            //transform.position =  _position.Position.Location;
            //LookInDirection(_position.Position.ViewDirection);
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

        private void OnCast()
        {

        }

        private void OnDeath()
        {

        }

        private void OnRessurect()
        {

        }
    }
}
