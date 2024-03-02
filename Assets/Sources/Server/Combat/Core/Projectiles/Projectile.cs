using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Core.Combat.Units;
using Core.Combat.Utils.MovementControllers;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Projectiles
{
    internal class Projectile : Updatable
    {
        private MovementController _movementController;
        private ProjectileHitService _hitService;
        private bool _destoyOnHit;
        private ProjectileModel _model;

        private class ProjectileModel
        {
            public Vector3 Position;
            private Vector3 _direction;
            private SpellId _hitSpell;
        }

        public void Update(IActionRecordContainer container)
        {
            _model.Position += _movementController.GetMovement(ModelUpdate.UpdateTime);

        }
    }

    internal interface ProjectileHitService
    {
        IEnumerable<Unit> GetHits(Vector3 position, Vector3 lastMovevent);
    }

    internal class LinearProjectileHitService : ProjectileHitService
    {
        private readonly float _radius;

        IEnumerable<Unit> ProjectileHitService.GetHits(Vector3 position, Vector3 lastMovement)
        {
            throw new System.NotImplementedException();
        }

        private static bool UnitHits(Vector3 position, Vector3 base1Center, Vector3 base2Center, float radiusSqr)
        {
            base2Center -= base1Center;
            position -= base1Center;

            float distance1Sqr = position.sqrMagnitude;
            float distance2Sqr = (position - base2Center).sqrMagnitude;
            float widthSqr = base2Center.sqrMagnitude;

            if (widthSqr + distance1Sqr < distance2Sqr || widthSqr + distance2Sqr < distance1Sqr)
            {
                return false;
            }

            float areaSqr = radiusSqr * widthSqr;

            float areaOfHitPointSqr = ((position - base1Center) * (position - base2Center)).sqrMagnitude;

            return areaSqr <= areaOfHitPointSqr;
        }
    }

    public class RadialProjectileHitService : ProjectileHitService
    {
        private Vector3 _startPoint;
        private float _widthSqr;

        public RadialProjectileHitService(Vector3 startPoint, float width)
        {
            width /= 2;
            _startPoint = startPoint;
            _widthSqr = width * width;
        }

        public IEnumerable<Unit> GetHits(Vector3 position, Vector3 lastMovevent)
        {
            float outerRadiusSqr = (position - _startPoint).sqrMagnitude;
            float innerRadiusSqr = (position - _startPoint - lastMovevent).sqrMagnitude;

            if (innerRadiusSqr < _widthSqr)
            {
                innerRadiusSqr = _widthSqr;
            }

            Unit[] result = Engine.Units.FindUnitsByPosition((position) => UnitHits(position, _startPoint, innerRadiusSqr, outerRadiusSqr, _widthSqr));
            return result;
        }

        private static bool UnitHits(Vector3 position, Vector3 startPosition, float innerRadiusSqr, float outerRadiusSqr, float widthSqr)
        {
            bool result;
            float distanceSqr = (position - startPosition).sqrMagnitude;
            float valueToSqr = innerRadiusSqr + widthSqr - distanceSqr;

            result = valueToSqr < 0 || (valueToSqr * valueToSqr <= 4 * widthSqr * innerRadiusSqr);
            valueToSqr = distanceSqr - outerRadiusSqr - widthSqr;
            result &= valueToSqr < 0 || (valueToSqr * valueToSqr <= 4 * widthSqr * outerRadiusSqr);

            return result;
        }
    }
}
