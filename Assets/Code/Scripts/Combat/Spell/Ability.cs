using Abilities;
using Data;
using UnityEngine;

public enum DamageCategory {
    DAMAGE_CATEGORY_SPELL,
    DAMAGE_CATEGORY_ATTACK
}

public enum DamageFlags: int {
    NONE = 0,
    NON_LETHAL = 1,
    HPLOSS = 2,
    BYPASS_EVADE = 4,
    BYPASS_PARRY = 8,
    BYPASS_BLOCK = 16,
    DOT_EFFECT = 32
}

public enum attackfail : int {
    NONE,
    MISS,
    PARRY,
    BLOCK,
    INVULNERABLE
}

public enum UNIT_TARGET_FLAGS : int {
    NONE = 0,
    RANGED_ONLY = 2,
    MELEE_ONLY = 4,
    DEAD = 8,
    NO_INVIS = 16
}

public enum UNIT_TARGET_TEAM {
    NONE,
    FRIENDLY,
    ENEMY,
    BOTH,
    CUSTOM
}

public enum AbilityBehavior : int{
    NONE = 0,
    HIDDEN = 1,
    PASSIVE = 2,
    NO_TARGET = 4,
    UNIT_TARGET = 8,
    POINT = 16,
    AOE = 32,
    INSTANT = 64,
    CAST_WHILE_DEAD = 128,
    CHANNALABLE = 256,
}

public enum AbilityResource : int {
    RESOURCE_NONE = -1,
    RESOURCE_LEFT = 0,
    RESOURCE_RIGHT = 1,
}

public enum UnitFilterResult {
    UF_SUCCESS,
    UF_FAIL_FRIENDLY,
    UF_FAIL_ENEMY,
    UF_FAIL_MELEE,
    UF_FAIL_RANGED,
    UF_FAIL_DEAD,
    UF_FAIL_INVISIBLE,
    UF_FAIL_INVALID_LOCATION
}

public abstract class Ability : ICastable, IChannelable{
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
    

    public readonly ushort DataID;

    //Runtime var
    private float _cooldown = 0;
    private float _cast = 0;
    private float _channel = 0;
    private Sprite _abilityIcon;
    private bool _onCooldown = false;
    private bool _frozen = false;

    public Sprite AbilityIcon => _abilityIcon;

    public Ability(Unit owner) {
        
        if (AbilityData == null ) {
            throw new MissingComponentException("No such ability data");
        }
        Owner = owner;
        _abilityIcon = AbilityData.icon;
        DataID = AbilityData.AbilityId;
        if (GetPassiveStatusName()!=null) {
            owner?.AddNewStatus(owner, this, GetPassiveStatusName(), new System.Collections.Generic.Dictionary<string, float>());
        }
    }
     
    public void Update(float deltaTime) {
        if (_onCooldown && !_frozen) {
            _cooldown -= deltaTime;
            if (_cooldown <= 0) {
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
    
    public void EndCooldown() {
        _cooldown = 0;
        _onCooldown = false;
    }
    
    public int Index => 0; //TODO: not implemented

    public virtual string GetAbilityName() => GetType().Name;
    
    public AbilityBehavior Behavior => AbilityBehavior;
    
    public Unit Caster => Owner;
    
    //Return cooldown without gcd
    public float Cooldown => _cooldown;

    public float GetActualCooldown() => Mathf.Max(_cooldown, Owner.Gcd); 
    
    public float GetGlobalCooldown() {
        if (Owner == null) return 0;
        return Owner.Gcd;
    }
    
    public float GetCooldownTimeRemaining() =>Mathf.Max(_cooldown, GetGlobalCooldown());
    
    public Vector3 CursorPosition() => Input.mousePosition;
    
    public virtual string GetPassiveStatusName() { return null; }
    
    public float ManaCost => AbilityCost; 
    
    public bool Channeling => _cast > 0;
    
    public bool IsCooldownReady() { return !_onCooldown; }
    //TODO: public bool IsFullyCastable() { return true; }
 
    
    public virtual void OnConcentrationStart() { }
    
    public virtual void OnOwnerDied() { }
    
    public virtual void OnOwnerSpawned() { }
    
    public virtual bool OnProjectileHit(Unit target, Vector3 location)  { return true; }
    //TODO: OnProjectileHit_ExtraData(target | nil, location, extraData)
    //TODO: OnProjectileHitHandle(target | nil, location, projectile)
    
    public virtual void OnProjectileThink(Vector3 location) { }
    //TODO: OnProjectileThink_ExtraData(location: Vector, extraData: table): nil
    //TODO: OnProjectileThinkHandle(projectileHandle: ProjectileID): nil

    public virtual void OnSpellStart() { }

    public void PayManaCost() {
        if (AbilityResource == AbilityResource.RESOURCE_NONE) {
            return;
        }
        Owner.SpendMana(ManaCost, (int)AbilityResource);
    }
    
    public void ResetCooldown() {
        _cooldown = 0;
        //cooldownImage.fillAmount = 0;
        _onCooldown = false;
    }
    
    public void SetFrozenCooldown(bool frozen) { _frozen = frozen; }
    
    public void StartCooldown() {
        if (AbilityCooldown > 0) {
            _cooldown = AbilityCooldown;
            _onCooldown = true;
        }
    }

    //Begin ability if cast is possible
    public virtual UnitFilterResult StartAbility() {

        return UnitFilterResult.UF_SUCCESS; }

}
