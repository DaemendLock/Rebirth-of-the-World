using Combat.SpellOld;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles {
    public abstract class ProjectileID {
        public int id;
        protected string name;
        protected OldAbility ability;
        protected Unit source;
        protected Vector3 location;
        protected Vector3 direction;
        protected float moveSpeed;
        protected float expire;
        protected Dictionary<string, float> data;
        protected GameObject particle;

        public ProjectileID(Vector3 location, float movespeed, float expire, string name, OldAbility ability, Unit source, Dictionary<string, float> data) {
            this.location = location;
            moveSpeed = movespeed;
            this.expire = expire;
            particle = GameObject.Instantiate(ProjectileManager.particleObject, location, Quaternion.Euler(direction), ProjectileManager.projectileContainer.transform);
            particle.GetComponent<Particle>().linkedProjectile = this;
            this.ability = ability;
            this.source = source;
            this.data = data;
            this.name = name;
        }

        public virtual bool Update(float time) {

            expire -= time;
            if (expire < 0) {
                Destroy();
                return true;
            }
            location += direction * moveSpeed * time;
            particle.transform.position = location;
            ability.OnProjectileThink(location);
            return false;
        }

        public void UpdateDestination(Vector3 destination) {
            direction = destination - location;
            particle.transform.rotation = Quaternion.LookRotation(direction);
        }

        public void Destroy() {
            UnityEngine.Object.Destroy(particle);
        }

        public abstract void Hit(Unit unit);
    }

    public class TrackingProjectile : ProjectileID {

        private Unit _target;
        private bool _attack;
        private bool _ignorCheck;
        private float _halfspeed;
        private bool _hit = false;
        public TrackingProjectile(Vector3 location, Unit target, float movespeed, float expire, bool attack, bool ignorCheck, string name, OldAbility ability, Unit source, Dictionary<string, float> data) : base(location, movespeed, expire, name, ability, source, data) {
            _target = target;
            _attack = attack;
            _ignorCheck = ignorCheck;
            _halfspeed = movespeed / 2;
            UpdateDestination(target.Origin);
        }

        public override bool Update(float time) {
            UpdateDestination(_target.Origin);
            if (base.Update(time))
                return true;
            return _hit;
        }

        public override void Hit(Unit unit) {
            if (unit == _target) {
                if (_attack) {
                    source.PerformAttack(_target, true, true);
                    _hit = true;
                } else
                    _hit = ability.OnProjectileHit(unit, location);
            }
        }
    }

    public class LinearProjectile : ProjectileID {
        private float _distance;
        private float _checkRadius;
        private UnitTargetTeam _team;
        private bool _hit = false;

        public LinearProjectile(Vector3 location, Vector3 direction, int movespeed, float maxDistance, float checkRadius, float expire, UnitTargetTeam team, string particleName, OldAbility ability, Unit source, Dictionary<string, float> data) : base(location, movespeed, expire, particleName, ability, source, data) {
            this.direction = direction;
            _distance = maxDistance;
            _checkRadius = checkRadius;
            _team = team;
        }

        public override void Hit(Unit unit) {
            if (ability.OnProjectileHit(unit, location))
                _hit = true;
        }
    }

    public static class ProjectileManager {

        public static GameObject particleObject = Resources.Load<GameObject>("Prefabs/Projectile");
        public static GameObject projectileContainer;

        private static List<ProjectileID> allProjectile = new List<ProjectileID>();

        public static void ChangeTrackingProjectileSpeed(OldAbility ability, int speed) {

        }
        public static void CreateLinearProjectile(Vector3 vSpawnOrigin, Vector3 vVelocity, int iMoveSpeed = 0,
                                                    float fDistance = 100, float fRadius = 0, float fExpireTime = 5,
                                                    UnitTargetTeam tTargetTeam = UnitTargetTeam.NONE,
                                                    string sEffectName = null, OldAbility ability = null, Unit source = null,
                                                    Dictionary<string, float> data = null) {
            allProjectile.Add(new LinearProjectile(vSpawnOrigin, vVelocity, iMoveSpeed, fDistance, fRadius, fExpireTime, tTargetTeam, sEffectName, ability, source, data));
        }
        public static void CreateTrackingProjectile(Vector3 vSpawnOrigin, Unit target, float fMoveSpeed = 0,
                                                    float fExpireTime = 5, bool bIsAttack = false, bool bSuppressTargetCheck = false,
                                                    string sEffectName = null, OldAbility ability = null, Unit source = null,
                                                    Dictionary<string, float> data = null) {
            allProjectile.Add(new TrackingProjectile(vSpawnOrigin, target, fMoveSpeed, fExpireTime, bIsAttack, bSuppressTargetCheck, sEffectName, ability, source, data));
        }

        public static void ProccesParticles(float time) {
            for (int i = 0; i < allProjectile.Count; i++) {
                if (allProjectile[i].Update(time)) {
                    allProjectile[i].Destroy();
                    allProjectile.RemoveAt(i);
                }
            }
        }
        public static void DestroyProjectile(ProjectileID projectile) {
            //projectile.Destroy();
        }
    }
}