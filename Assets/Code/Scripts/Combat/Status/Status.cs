using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum modifierfunction {
    MODIFIER_EVENT_ON_ABILITY_END_CHANNEL,
    MODIFIER_EVENT_ON_DEATH,
    MODIFIER_EVENT_ON_DAMAGE,
    MODIFIER_EVENT_ON_HEAL,
    MODIFIER_PROPERTY_HASTE_BONUS,
    MODIFIER_PROPERTY_MOVESPEED_BONUS,
    MODIFIER_PROPERTY_HASTE_BONUS_PERCENT,
    MODIFIER_PROPERTY_ATTACK_BONUS,
    MODIFIER_PROPERTY_ATTACK_BONUS_PERCENT,
    MODIFIER_PROPERTY_SPELLPOWER_BONUS,
    MODIFIER_PROPERTY_SPELLPOWER_BONUS_PERCENT,
    MODIFIER_PROPERTY_RECEIVE_DAMAGE_PERCENT,
    MODIFIER_PROPERTY_DAMAGE_PREBLOCK,
    MODIFIER_PROPERTY_DAMAGE_BLOCK
}

public enum modifierstate {
    MODIFIER_STATE_ROOTED,
    MODIFIER_STATE_DISARMED,
    MODIFIER_STATE_SILENCED,
    MODIFIER_STATE_STUNNED,
    MODIFIER_STATE_INVISIBLE,
    MODIFIER_STATE_BLIND,
    MODIFIER_STATE_TAUNTED
}

public abstract class Status {

    public static Dictionary<string, Type> allStatusList = new Dictionary<string, Type>();

    public event Action<float> DurationChanged;
    public event Action Destroied;
    public event Action Expired;

    private Dictionary<string, float> _data;
    public float Duration { get; private set; }
    public float StackCount { get; protected set; } = 0;
    private bool _stopDuration = false;
    private float _durationLeft;
    private float _startDuration;
    private Unit _caster;
    private Unit _owner;
    private Ability _ability;
    private bool destroyExpire = true;
    private float timer = 0;
    private float timeLeft = 0;

    public Status(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) {
        _owner = owner;
        _caster = caster;
        _ability = ability;
        SetDuration(data.GetValueOrDefault("duration", -1));
        _data = data;
        _startDuration = data.GetValueOrDefault("duration", 0);
        if (DeclareFunctions().Contains(modifierfunction.MODIFIER_EVENT_ON_DAMAGE))
            owner.RecivedDamage += OnDamage;
        if (DeclareFunctions().Contains(modifierfunction.MODIFIER_EVENT_ON_HEAL))
            owner.RecivedDamage += OnHeal;
        OnCreated(data);
    }

    


    public void ForceRefresh(Dictionary<string, float> data = null) {
        if(data != null)
        SetDuration(data.GetValueOrDefault("duration", -1));
        OnRefresh(data);
        _owner.UpdateStatusBonuses(this);
    }

    public void Update(float deltaTime) {
        if (timer != 0) {
            timeLeft -= deltaTime;
            
            while(timeLeft<= 0) {
                OnIntervalThink();
                timeLeft += timer;
            }        }
        if (_stopDuration) { return; }
        _durationLeft -= deltaTime;
        if (_durationLeft <= 0) {
            Expired?.Invoke();
            Remove();
            return;
        }
        
    }

    public void StartIntervalThink(float time) {
        if (time > 0) {
            timer = time;
            timeLeft = time;
            return;
        }
        timer= 0;
        timeLeft = 0;
    }

    public Ability Ability => _ability;

    public float GetFullDuration() => Duration;
    

    public float GetStartDuration() => _startDuration;
    

    public void SetDuration(float duration) {
        if (duration <= 0) {
            _stopDuration = true;
            Duration = -1;
        }
        Duration = duration;
        _durationLeft = duration;
        DurationChanged?.Invoke(duration);
    }

    public void StopDuration() {
        _stopDuration = true;
    }

    public void ContinueDuration() {
        _stopDuration = false;
    }

    private void Destroy() {
        Duration = 0;
        if (DeclareFunctions().Contains(modifierfunction.MODIFIER_EVENT_ON_DAMAGE))
            _owner.RecivedDamage -= OnDamage;
        if (DeclareFunctions().Contains(modifierfunction.MODIFIER_EVENT_ON_HEAL))
            _owner.RecivedDamage -= OnHeal;
        _owner.ForceRemove(this);
        OnDestroy();
        Destroied?.Invoke();
    }

    public void Remove() {
        _owner.ForceRemove(this);
        OnRemoved();
        Destroy();
    }

    public virtual Dictionary<modifierstate, bool> CheckState() {
        return null;
    }

    public bool DestroyOnExpire() {
        return destroyExpire;
    }
    
    public virtual float GetAuraDuration() {
        return 0;
    }
    
    public virtual bool GetAuraEntityReject() {
        return false;
    }
    
    public virtual float GetAuraRadius() {
        return 0;
    }
    
    public virtual UNIT_TARGET_FLAGS GetAuraSearchFlags() {
        return UNIT_TARGET_FLAGS.NONE;
    }
    
    public virtual UNIT_TARGET_TEAM GetAuraSearchTeam() {
        return UNIT_TARGET_TEAM.NONE;
    }
    
    public Unit Caster => _caster; 
    public string Name => nameof(this.GetType); 
    
    public Unit Parent => _owner;
    
    
    public float GetRemainingTime() {
        if (Duration == -1) return 0;
        return _durationLeft;
    }
    
    public virtual string GetEffectName() {
        return nameof(this.GetType);
    }
    
    public Status GetModifierAura() {
        return null;
    }
    
    public virtual Sprite GetIcon() { return Ability.AbilityIcon; }
   
    public void IncreaseStackCount(int count, bool refresh = false) {
        StackCount = count;
    }

    public virtual bool IsAura() { return false; }
    
    public virtual bool IsDebuff() { return false; }
    
    public virtual bool IsHidden() { return false;}
    
    public virtual bool IsPurgable() { return false; }
    
    public virtual bool IsStunDebuff() { return false; }

    public virtual void OnCreated(Dictionary<string, float> param) { OnCreated(); }
    
    public virtual void OnCreated() { }
    
    public virtual void OnDestroy() { }
    
    public virtual void OnIntervalThink() { }
    
    public virtual void OnRefresh(Dictionary<string, float> param) { }
    
    public virtual void OnRemoved() { }
    
    public virtual void OnStackCountChanged() { }
    
    public virtual bool RemoveOnDeath() { return true; }
    
    public virtual modifierfunction[] DeclareFunctions() { return new modifierfunction[0]; }
    
    public virtual bool GetDisableHealing() { return false; }
    
    public virtual float GetMinHealth() { return 0; }

    public virtual float GetModifierExtraHealthBonus() { return 0; }
    
    public virtual float GetModifierExtraHealthPercentage() { return 0; }
    
    public virtual float GetModifierHasteBonus() => 0;

    public virtual float GetModifierHasteBonus_Percent() => 0;

    public virtual float GetModifierMovespeedBinus() => 0;

    public virtual float GetModifierAttack_Bonus() { return 0; }

    public virtual float GetModifierAttack_Bonus_Percent() { return 0; }

    public virtual float GetModifierSpellpower_Bonus() { return 0; }

    public virtual float GetModifierSpellpower_Bonus_Percent() { return 0; }

    public virtual float GetModifierReceiveDamage_Percent(AttackEventInstance e) {
        return 100;
    }

    public virtual void OnAbilityEndChannel() { }

    public virtual void OnDamage(AttackEventInstance damageEvent) { }
    
    public virtual void OnHeal(AttackEventInstance damageEvent) { }
    
    public virtual void OnDeath(AttackEventInstance damageEvent) { }
    
    public virtual void OnHealReceived(DamageEvent healEvent) { }

    public virtual float GetModifierDamagePreblock(AttackEventInstance e) {
        return 0;
    }

    public virtual float GetModifierDamageBlock(AttackEventInstance e) {
        return 0;
    }
}