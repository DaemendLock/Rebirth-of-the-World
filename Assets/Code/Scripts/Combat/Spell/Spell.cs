using Data;
using System;
using UnityEngine;

namespace Combat.SpellOld
{
    [Flags]
    public enum UnitTargetFlag
    {
        NONE,
        RANGED_ONLY,
        MELEE_ONLY,
        DEAD,
        NO_INVIS
    }

    public enum UnitTargetTeam
    {
        NONE,
        FRIENDLY,
        ENEMY,
        BOTH,
        CUSTOM
    }

    [Flags]
    public enum AbilityBehavior : int
    {
        NONE,
        HIDDEN,
        PASSIVE,
        NO_TARGET,
        UNIT_TARGET,
        POINT,
        AOE,
        INSTANT,
        CAST_WHILE_DEAD,
        CHANNALABLE,
    }

    public enum AbilityResource : sbyte
    {
        NONE = -1,
        LEFT = 0,
        RIGHT = 1,
    }

    public enum UnitFilterResult
    {
        UF_SUCCESS,
        UF_FAIL_FRIENDLY,
        UF_FAIL_ENEMY,
        UF_FAIL_MELEE,
        UF_FAIL_RANGED,
        UF_FAIL_DEAD,
        UF_FAIL_INVISIBLE,
        UF_FAIL_INVALID_LOCATION
    }

    public abstract class OldAbility
    {
        //Base data
        public readonly Unit Owner;

        public abstract AbilityData AbilityData { get; }
        public abstract float AbilityCooldown { get; }
        public abstract float CastTime { get; }
        public abstract float ChannelTime { get; }
        public abstract AbilityBehavior AbilityBehavior { get; }
        public abstract AbilityResource AbilityResource { get; }
        public abstract float AbilityCost { get; }
        public abstract float AbilityDamage { get; }
        public abstract ushort SpellId { get; }

        public abstract bool ShowOnTooltip { get; }

        public readonly ushort DataID;

        //Runtime var
        private float _cooldownEnd = 0;    //When ability goes off cooldown
        private float _cast = 0;        //Cast time left
        private float _channel = 0;     //Channel time left
        private Sprite _abilityIcon;
        private bool _frozen = false;   //stop cooldown

        public Sprite AbilityIcon => _abilityIcon;

        public OldAbility(Unit owner)
        {
            Owner = owner;
            _abilityIcon = AbilityData.icon;
            DataID = AbilityData.AbilityId;

            if (GetPassiveStatusName() != null)
            {
                owner?.AddNewStatus(owner, this, GetPassiveStatusName(), new System.Collections.Generic.Dictionary<string, float>());
            }
        }

        public void Update(float deltaTime)
        {
            if (_frozen)
            {
                _cooldownEnd += deltaTime;

                if (_cooldownEnd <= 0)
                {
                    ResetCooldown();
                }

                return;
            }
        }

        public bool CastAbility() => Owner.Alive && GetActualCooldown() == 0 && !Owner.Casting && !Owner.Channeling && Owner.GetResource((int) AbilityResource) >= AbilityCost && !(Castable && Owner.Moving || Channelable && Owner.Moving) && StartAbility() == UnitFilterResult.UF_SUCCESS;

        #region Castable
        public bool Castable => CastTime > 0;

        public virtual void OnCastStart() { }

        public virtual void OnCastThink(float casttime) { }

        public virtual void OnCastFinished(bool succes) { }
        #endregion

        #region Channelable

        public bool Channelable => ChannelTime > 0;

        public virtual void OnChannelStart() { }

        public virtual void OnChannelThink(float time) { }

        public virtual void OnChannelFinished(bool success) { }

        #endregion

        public virtual UnitFilterResult CastFilterResult() => UnitFilterResult.UF_SUCCESS;

        public virtual UnitFilterResult CastFilterResultLocation(Vector3 location) => UnitFilterResult.UF_SUCCESS;

        public void EndCooldown()
        {
            _cooldownEnd = 0;
        }

        public int Index => 0; //TODO: not implemented

        public virtual string GetAbilityName() => GetType().Name;

        public AbilityBehavior Behavior => AbilityBehavior;

        public Unit Caster => Owner;

        //Return cooldown without gcd
        public float Cooldown => Math.Max(_cooldownEnd - Time.time, 0);

        public float GetActualCooldown() => Mathf.Max(_cooldownEnd, Owner.Gcd);

        public float GetGlobalCooldown()
        {
            if (Owner == null)
                return 0;
            return Owner.Gcd;
        }

        public float GetCooldownTimeRemaining() => Mathf.Max(_cooldownEnd, GetGlobalCooldown());

        public Vector3 CursorPosition() => Input.mousePosition;

        public virtual string GetPassiveStatusName() { return null; }

        public float ManaCost => AbilityCost;

        public bool Channeling => _cast > 0;

        public bool IsCooldownReady() { return Time.time > _cooldownEnd; }
        //TODO: public bool IsFullyCastable() { return true; }

        public virtual void OnConcentrationStart() { }

        public virtual void OnOwnerDied() { }

        public virtual void OnOwnerSpawned() { }

        public virtual bool OnProjectileHit(Unit target, Vector3 location) { return true; }
        //TODO: OnProjectileHit_ExtraData(target | nil, location, extraData)
        //TODO: OnProjectileHitHandle(target | nil, location, projectile)

        public virtual void OnProjectileThink(Vector3 location) { }
        //TODO: OnProjectileThink_ExtraData(location: Vector, extraData: table): nil
        //TODO: OnProjectileThinkHandle(projectileHandle: ProjectileID): nil

        public virtual void OnSpellStart() { }

        public void PayManaCost()
        {
            if (AbilityResource == AbilityResource.NONE)
            {
                return;
            }
            Owner.SpendMana(ManaCost, (int) AbilityResource);
        }

        public void ResetCooldown()
        {
            _cooldownEnd = 0;
        }

        public void SetFrozenCooldown(bool frozen) { _frozen = frozen; }

        public void StartCooldown()
        {
            if (AbilityCooldown > 0)
            {
                _cooldownEnd = AbilityCooldown;
            }
        }

        //Begin ability if cast is possible
        public virtual UnitFilterResult StartAbility()
        {
            return UnitFilterResult.UF_SUCCESS;
        }
    }
}