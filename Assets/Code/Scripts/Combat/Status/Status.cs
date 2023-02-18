using System;
using System.Collections.Generic;
using UnitOperations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum modifierfunction : int {
    MODIFIER_EVENT_ON_ABILITY_END_CHANNEL,
    MODIFIER_EVENT_ON_DEATH,
    MODIFIER_EVENT_ON_DAMAGE,
    MODIFIER_EVENT_ON_HEAL,
    MODIFIER_PROPERTY_DAMAGE_RECIVE_PERCENT,
    MODIFIER_PROPERTY_DAMAGE_RECIVE_CONSTANT
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

    #region Events
    public event Action<float> DurationChanged;
    public event Action Destroied;
    public event Action Expired;
    public event Action<Status> Refreshed;
    #endregion

    private readonly Dictionary<string, float> _data;
    public float Duration { get; private set; }
    public float StackCount { get; protected set; } = 0;
    private readonly float _startDuration;
    private readonly Unit _caster;
    private readonly Unit _owner;
    private readonly Ability _ability;
    private readonly bool _destroyExpire = true;
    private float _timer = 0;
    private float _timeLeft = 0;
    private float _durationLeft;
    private bool _stopDuration = false;
    protected readonly long _affectedEvents = 0;

    public abstract StatsTable Bonuses { get; }

    public Status(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data, long affectedEvents = 0) {
        _owner = owner;
        _caster = caster;
        _ability = ability;
        _affectedEvents = affectedEvents;
        SetDuration(data.GetValueOrDefault("duration", -1));
        _data = data;
        _startDuration = data.GetValueOrDefault("duration", 0);
        OnCreated(data);
    }

    public void ForceRefresh(Dictionary<string, float> data = null) {
        if (data != null)
            SetDuration(data.GetValueOrDefault("duration", -1));
        Refreshed?.Invoke(this);
        OnRefresh(data);
    }

    public void Update(float deltaTime) {
        if (_timer != 0) {
            _timeLeft -= deltaTime;

            while (_timeLeft <= 0) {
                OnIntervalThink();
                _timeLeft += _timer;
            }
        }
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
            _timer = time;
            _timeLeft = time;
            return;
        }
        _timer = 0;
        _timeLeft = 0;
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
        _owner.ForceRemove(this);
        OnDestroy();
        Destroied?.Invoke();
    }

    public void Remove() {
        OnRemoved();
        Destroy();
    }

    public virtual Dictionary<modifierstate, bool> CheckState() {
        return null;
    }

    public bool DestroyOnExpire() {
        return _destroyExpire;
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
        if (Duration == -1)
            return 0;
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

    public virtual bool IsHidden() { return false; }

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

    public virtual bool GetDisableHealing() { return false; }

    public virtual float GetMinHealth() { return 0; }

    public bool AffectsDamageRecived => (_affectedEvents & ((long)modifierfunction.MODIFIER_PROPERTY_DAMAGE_RECIVE_PERCENT | (long)modifierfunction.MODIFIER_PROPERTY_DAMAGE_RECIVE_CONSTANT )) != 0;

    public virtual float GetDamageRecivePercentBonus(AttackEventInstance e) {
        return 0;
    }

    public virtual float GetBlockConstant(AttackEventInstance e) {
        return 0;
    }
}

public abstract class OvershieldStatus : Status {

    public event Action<OvershieldStatus> ShieldBroke;
    public event Action<float> DurabilityUpdated;

    private float _durability;
    private bool _destroyOnBreak;
    public OvershieldStatus(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data, float durability, bool destroyOnBroken) : base(owner, caster, ability, data){
        _durability = durability;
        Destroied += Break;
        _destroyOnBreak = destroyOnBroken;
    }

    public float Durability => _durability;

    //return True when shield breaks
    public bool Damage(AttackEventInstance e) {
        if (e.Value < _durability) {
            _durability -= e.Value;
            e.Value = 0;
            return false;
        }
        e.Value -= _durability;
        Break();
        return !_destroyOnBreak;
    }

    public void ForceRefresh(float newDurability, Dictionary<string, float> data = null) {
        SetDurability(newDurability);
        ForceRefresh(data);
    }

    public void SetDurability(float newDurability) {
        if (newDurability < 0) {
            Remove();
            return;
        }
        float dif = newDurability - _durability;
        _durability = newDurability;
        DurabilityUpdated?.Invoke(dif);
        
    }

    public void ChangeDurability(float durabilityChange) {
        _durability += durabilityChange;
        if (_durability < 0) {
            Remove();
            return;
        }
        DurabilityUpdated?.Invoke(durabilityChange);
    }

    private void Break() {   
        _durability = 0;
        if (!_destroyOnBreak) {
            return;
        }
        ShieldBroke?.Invoke(this);
        Destroied -= Break;
    }

}