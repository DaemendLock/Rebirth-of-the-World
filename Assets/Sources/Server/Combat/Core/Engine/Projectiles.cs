using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Projectiles;
using Core.Combat.Units;
using System;
using Utils.DataTypes;
using Utils.Patterns.Factory;

namespace Core.Combat.Engine
{
    internal static class Projectiles
    {
        private class ProjectileFactory : Factory<Projectile>
        {
            public int NextId;

            Projectile Factory<Projectile>.Create()
            {
                throw new NotImplementedException();
                //return new Projectile(NextId);
            }
        }

        private static ProjectileFactory _factory;

        internal static Projectile CreateLinearProjectile(Unit owner, Vector3 initialPosition, Vector3 direction, float speed)
        {
            //_factory.NextId = _entityList.NextId;
            throw new NotImplementedException();
        }

        internal static Projectile CreateTargetedProjectile(Unit owner, Vector3 initialPosition, Unit target, float speed)
        {
            //MovementController controller = new TargetedProjectileMovementController();
            throw new NotImplementedException();
        }

        internal static void Update(IActionRecordContainer container)
        {

        }
    }
}
