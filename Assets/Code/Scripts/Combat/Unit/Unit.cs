using Data;
using Events;
using Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitOperations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UnitStats {
    ATK,
    ATK_PERCENT,
    SPELLPOWER,
    SPELLPOWER_PERCENT,
    HASTE,
    CRIT,
    VERSA,
    MAXHP,
    MAXMANA,
    LIFESTEAL,
    MSPEED,
    AOERESIST,
    BLOCK,
    EVADE,
    PARRY
}

public enum Team {
    TEAM_ALLY,
    TEAM_ENEMY,
    TEAM_NOTEAM,
}

public abstract class Unit : MonoBehaviour, IPointerClickHandler, IHealthOwner, ICaster {
    //Table to maintain all exsisting Units. Require for CreateUnit(<name>);
    public static Dictionary<string, Type> allUnitTypes = new Dictionary<string, Type>();

    //Events declaration
    #region Events
    //Health events:
    public event Action<float> HealthChanged;
    public event Action<AttackEventInstance> RecivedDamage;
    public event Action<AttackEventInstance> Healed;
    public event Action Died;
    public event Action<float> Resurected;

    //Fired on team Changed
    public event Action<Unit, Team> TeamChange;

    //Cast events:
    public event Action<Ability> AbilityUsed;
    public event Action<Ability> CastStarted;
    public event Action<float> CastProcessed;
    public event Action<Ability, bool> CastEnded;
    public event Action<Ability> ChannelStarted;
    public event Action<float> ChannelProcessed;
    public event Action<Ability, bool> ChannelEnded;

    //Status events:
    public event Action<Status> StatusApplied;
    public event Action<Status> StatusRemoved;
    #endregion

    #region Variables
    [SerializeField] private UnitData baseUnit;

    //Key Values
    private float[] baseResourceRegen = new float[2];

    private readonly string _unitLabel;
    private readonly float _turnRate;

    //ColorData
    public Color leftColor = new Color(0, 128, 255);
    public Color rightColor = new Color(127, 0, 255);

    //Positioning
    private Vector3 _location;
    private float _ringRadius;
    private Vector3 _moveTo;
    private bool _moving = false;

    //Define Stat Tables
    private StatsTable _baseStats;
    private StatsTable _equipBonus = StatsTable.FromDictionary(new Dictionary<UnitStats, float>(){
        { UnitStats.ATK, 0 },       //ATK
        {UnitStats.SPELLPOWER, 0 }, //SPW
        { UnitStats.HASTE, 0 },     //HST
        { UnitStats.CRIT, 0 },      //CRT
        { UnitStats.VERSA, 0 },     //VRS
        { UnitStats.MAXHP, 0 },     //MHP
        { UnitStats.MAXMANA, 0 },
        { UnitStats.LIFESTEAL, 0 }, //LST
        { UnitStats.MSPEED, 0},     //SPD
        { UnitStats.AOERESIST, 0 }, //RES
        { UnitStats.BLOCK, 0 },
        { UnitStats.EVADE, 0 },
        { UnitStats.PARRY, 0 }});
    private StatsTable _totalStats = StatsTable.FromDictionary(new Dictionary<UnitStats, float>(){
        { UnitStats.ATK, 0 },       //ATK
        { UnitStats.SPELLPOWER, 0 }, //SPW
        { UnitStats.SPELLPOWER_PERCENT, 100 },
        { UnitStats.HASTE, 0 },     //HST
        { UnitStats.CRIT, 0 },      //CRT
        { UnitStats.VERSA, 0 },     //VRS
        { UnitStats.MAXHP, 0 },     //MHP
        { UnitStats.MAXMANA, 0 },
        { UnitStats.LIFESTEAL, 0 }, //LST
        { UnitStats.MSPEED, 0},     //SPD
        { UnitStats.AOERESIST, 0 }, //RES
        { UnitStats.BLOCK, 0 },
        { UnitStats.EVADE, 0 },
        { UnitStats.PARRY, 0 }});

    //AI
    private AIModule ai;

    [SerializeReference] protected Ability[] abilities = new Ability[6];
    private Dictionary<GearSlot, Gear> gear = new Dictionary<GearSlot, Gear> {
        [GearSlot.HEAD] = null,
        [GearSlot.BODY] = null,
        [GearSlot.FEETS] = null,
        [GearSlot.LEFT_ARM] = null,
        [GearSlot.RIGHT_ARM] = null,
        [GearSlot.RING_1] = null,
        [GearSlot.RING_2] = null,
        [GearSlot.CONSUMABLE_1] = null,
        [GearSlot.CONSUMABLE_2] = null,
    };

    //Combat state
    [SerializeField] private Team team;
    private bool _alive = true;
    private float _hp;
    private float _overshield = 0;
    private readonly float[] _resource = new float[2];

    //Resources
    private float _maxHp;
    private float _minHp = 0;

    //Casting
    private Ability _cast;
    private bool _isCasting = false;
    private bool _isChannel = false;
    private float _castTime;
    private float _channelTime;
    private float _gcd = 0;
    private Ability _queuedAbility = null;
    private float[] _maxResource = new float[2];

    //Flags
    private bool _meele;

    //Velocity
    private float _moveSpeed;
    private float _attackDelay;
    private float _projectileSpeed;

    private Unit target;

    //Stats tracking
    private Dictionary<Status, StatsTable> _statusBonuses = new();

    public float RingRadius => _ringRadius;
    #endregion

    protected virtual void Precache() {

    }

    public abstract void Init();

    private void EvaluateBaseUnitStats() {
        _baseStats = baseUnit.CalculateUnitStatsByRankAndLevel(0, 0);
        _totalStats += _baseStats + _equipBonus;

        _maxResource[0] = baseUnit.maxResource[0];
        _maxResource[1] = baseUnit.maxResource[1];
        _resource[0] = _maxResource[0];
        _resource[1] = _maxResource[1];
        baseResourceRegen[0] = baseUnit.resourceRegen[0];
        baseResourceRegen[1] = baseUnit.resourceRegen[1];

        _meele = !baseUnit.RangedAttack;
        _projectileSpeed = baseUnit.ProjectileSpeed;

        _moveSpeed = _totalStats.MSPEED;
        _maxHp = _totalStats.MAXHP;
    }

    private void Start() {
        Precache();
        EvaluateBaseUnitStats();
        Init();
        _hp = _maxHp;

    }

    public void SetUnitState() {

    }

    #region Statuses
    private List<Status> statusList = new List<Status>();

    public List<Status> AllStatuses => new List<Status>(statusList);

    public void AddNewStatus(Unit caster, Ability ability, string name, Dictionary<string, float> data) {
        Type type = Status.allStatusList.GetValueOrDefault(name, null);
        if (type == null) {
            Debug.LogError("No such Status :" + name);
            return;
        }
        Status check = FindStatusByNameAndCaster(name, caster);
        if (check == null) {
            check = (Status) Activator.CreateInstance(type, this, caster, ability, data);
            statusList.Add(check);
            AddStatusBonuses(check);
            StatusApplied?.Invoke(check);        
        } else {
            check.ForceRefresh(data);
        }
        
        
    }

    public Status FindStatusByName(string name) {
        Type type = Status.allStatusList[name];
        foreach (Status s in statusList) {
            if (s.GetType() == type)
                return s;
        }
        return null;
    }

    public Status FindStatusByNameAndCaster(string name, Unit caster) {
        Type type = Status.allStatusList[name];
        foreach (Status s in statusList) {

            if (s.GetType() == type && s.Caster == caster) { return s; } else if (s.GetType() == type)
                Debug.Log(s.Caster==caster);
        }
        return null;
    }

    public List<Status> FindAllStatusesByName(string name) { return statusList.Where(s => s.Name.Equals(name)).ToList(); }

    public bool HasStatus(Status status) {
        Type t = status.GetType();
        foreach (Status s in statusList) {
            if (s.GetType() == t)
                return true;
        }
        return false;
    }

    public bool HasStatus(string status_name) {
        Type t;
        if (Status.allStatusList.TryGetValue(status_name, out t))
            return false;

        foreach (Status s in statusList) {
            if (t == s.GetType())
                return true;
        }
        return false;
    }

    public void RemoveAllStatusesOfName(string name) {
        foreach (Status s in FindAllStatusesByName(name)) {
            s.Remove();
        }
    }

    //Remove first status<name>
    public void RemoveStatusByName(string name) {
        FindStatusByName(name).Remove();
    }

    //Remove first status<name> inflicted by caster
    public void RemoveStatusByNameAndCaster(string name, Unit caster) {
        FindStatusByNameAndCaster(name, caster).Remove();
    }

    //Instantly removes status <s> if this unit is affected
    public void ForceRemove(Status s) {
        if (!statusList.Contains(s))
            return;
        RemoveStatusBonuses(s);
        statusList.Remove(s);
    }
    #endregion

    #region Casting
    public float CastTimeRemain => _castTime;

    public float ChannelTime => _channelTime;

    public bool Casting => _isCasting;

    public bool Channeling => _isChannel;

    public Ability CurrentCastAbility => _cast;

    public Ability QueuedAbility { get => _queuedAbility; }

    public void CastAbility(Ability ability) {
        if(!ability.CastAbility()) return;
        AbilityUsed?.Invoke(ability);
        EventManager.SendAbilityEvent(new AbilityEventInstance(ability));
        if (ability.Castable) {
            StartCasting(ability);
        } else if (ability.Channelable) {
            StartChannel(ability);
        }
    }

    public Ability FindAbilityByName(string abilityName) {
        return null;
    } //TODO: not implemented

    ///<summary>
    ///Return ability at slot
    ///</summary>
    public Ability GetAbilityByIndex(int index) => index >= abilities.Length ? null : abilities[index];

    public void Interrupt(bool succes = false) {
        if (_isCasting)
            StopCasting(succes);
        if (_isChannel)
            StopChannel(succes);
    }

    public void RefreshCD() {
        foreach (Ability ability in abilities)
            ability?.ResetCooldown();
    }

    private void ProcessCast(float time) {
        _castTime -= time;
        _cast?.OnCastThink(_castTime);
        CastProcessed?.Invoke(time);
        if (_cast != null && _castTime < 0) {
            StopCasting(true);
        }
    }

    private void StartCasting(Ability ability) {
        if (!ability.Castable)
            return;
        _isCasting = true;
        _cast = ability;
        _castTime = ability.CastTime * HasteCasttimeModification;
        ability.OnCastStart();
        CastStarted?.Invoke(ability);
    }

    private void StopCasting(bool succes) {
        _isCasting = false;
        target = null;
        _cast?.OnCastFinished(succes);
        CastEnded?.Invoke(_cast, succes);
        if (succes && _cast.Channelable) {
            StartChannel(_cast);
            return;
        }
        _cast = null;
        if (succes && QueuedAbility != null) {
            QueuedAbility.CastAbility();
            _queuedAbility = null;
        }
    }

    private void StartChannel(Ability ability) {
        _isChannel = true;
        _channelTime = 0;
        _cast = ability;
        ChannelStarted?.Invoke(ability);
        ability.OnChannelStart();
    }

    private void ProcessChannel(float deltaTime) {
        _channelTime += deltaTime;
        _cast?.OnChannelThink(ChannelTime);
        ChannelProcessed?.Invoke(deltaTime);
        
        if (_cast!= null && _channelTime >= _cast.ChannelTime) {
            StopChannel(true);
        }
    }

    private void StopChannel(bool succes) {
        _isChannel = false;
        _channelTime = 0;
        ChannelEnded?.Invoke(_cast, succes);
        _cast = null;
        
        if (succes && QueuedAbility != null) {
            QueuedAbility.CastAbility();
            _queuedAbility = null;
        }
    }

    #endregion

    #region Health
    public float HealthPercent => _hp / _maxHp;

    public float CurrentHealth => _hp;

    public float MaxHealth => _maxHp;

    public float MinHealth => _minHp;

    public void SetHealth(float health) {
        float healthWas = _hp;
        _hp = health;
        if (_hp < _minHp)
            _hp = _minHp;
        if (_hp > _maxHp)
            _hp = _maxHp;

        HealthChanged?.Invoke(healthWas);
    }

    //Killable
    public bool Alive => _alive;

    public bool Dead => !_alive;

    public void ForceKill(bool resurectable) {
        if (Dead) { throw new Exception("Can't kill dead unit"); }
        for (int i = statusList.Count - 1; i >= 0; i--)
            if (statusList[i].RemoveOnDeath())
                statusList.RemoveAt(i);
        Interrupt();
        transform.Rotate(90, 0, 0);
        _hp = 0;
        _minHp = 0;
        _alive = false;
        OnDeath();
        EventManager.SendDeathEvent(this);
    }

    public void Kill() {
        ForceKill(true);
    }

    public void Respawn() {
        ResurectWithHealth(_maxHp);
    }

    public void ResurectWithHealthPercent(float percent) {
        ResurectWithHealth(_maxHp * percent * 0.01f);
    }

    public void ResurectWithHealth(float health) {
        if (_alive)
            throw new Exception("Can't resurect alive unit");
        _alive = true;
        _hp = health;
        Resurected?.Invoke(health);
        EventManager.SendUnitResurectedEvent(this, health);
    }

    public virtual void OnDeath() { }

    //Damagable
    public float GetDamageReceive(AttackEventInstance e) {
        float res = 100;
        for (int i = 0; i < statusList.Count; i++) {
            if (statusList[i].DeclareFunctions().Contains(modifierfunction.MODIFIER_PROPERTY_RECEIVE_DAMAGE_PERCENT)) {
                res += statusList[i].GetModifierReceiveDamage_Percent(e);
            }
        }
        return res;
    }

    public void Damage(AttackEventInstance e) {
        if (!((e.damageFlags & (DamageFlags.BYPASS_EVADE | DamageFlags.DOT_EFFECT | DamageFlags.HPLOSS)) == 0) && RotW.RollPercentage(EvadeChance)) {
            e.fail_type = attackfail.MISS;
            return;
        }
        if (!((e.damageFlags & (DamageFlags.BYPASS_PARRY | DamageFlags.DOT_EFFECT | DamageFlags.HPLOSS)) == 0) && RotW.RollPercentage(ParryChance)) {
            e.damage = 0;
            e.fail_type = attackfail.PARRY;
            RecivedDamage?.Invoke(e);
            return;
        }
        foreach (Status s in statusList) {
            if (s.DeclareFunctions().Contains(modifierfunction.MODIFIER_PROPERTY_DAMAGE_PREBLOCK)) {
                if (e.damage < 0) {
                    return;
                }
                e.damage -= s.GetModifierDamagePreblock(e);
            }
        }

        e.damage = e.damage * GetDamageReceive(e) * 0.01f;
        if (!((e.damageFlags & DamageFlags.BYPASS_BLOCK) == 0) && RotW.RollPercentage(BlockChance)) {
            e.damage *= RotW.DAMAGE_BYPASS_BLOCK;
        }
        foreach (Status s in statusList) {
            if (s.DeclareFunctions().Contains(modifierfunction.MODIFIER_PROPERTY_DAMAGE_BLOCK)) {
                if (e.damage < 0) {
                    return;
                }
                e.damage -= s.GetModifierDamagePreblock(e);
            }
        }
        if (_hp - e.damage <= 0 && (e.damageFlags & DamageFlags.NON_LETHAL) != 0) {
            e.damage = _hp - 1;
        }
        _overshield -= e.damage;
        if (_overshield < e.damage) {
            _hp += _overshield;
            _overshield = 0;
        } else {
            _overshield -= e.damage;
        }
        _hp -= e.damage;
        if (_hp < _minHp) {
            _hp = _minHp;
        }

        RecivedDamage?.Invoke(e);
        if (_hp <= 0) {
            Kill();
        }
    }

    public virtual void OnDamage(AttackEventInstance e) { }

    //Healable
    public void Heal(float ammount, Ability ability) {
        if (ammount < 0)
            return;
        _hp += ammount;
        if (_hp > _maxHp) {
            _hp = _maxHp;
        }
        Healed?.Invoke(new AttackEventInstance(ability.Owner, -ammount, DamageCategory.DAMAGE_CATEGORY_SPELL, ability, -ammount, this, 0));
    }

    public float GetHealRecive() => 100;

    public virtual void OnHealRecived(AttackEventInstance e) { }
    #endregion

    #region Mana
    public float GetResource(int resource) => (resource < _resource.Length && resource >= 0) ? _resource[resource] : 0;

    public float GetResourceMax(int resource) => (resource < _maxResource.Length && resource >= 0) ? _maxResource[resource] : 0;

    public float GetResourcePercent(int resource) => (resource < _resource.Length && resource >= 0) ? _resource[resource] / _maxResource[resource] : 0;

    public int ResourceCount => _resource.Length;

    public void GiveMana(float ammount, AbilityResource takeResource) {
        GiveMana(ammount, (int) takeResource);
    }

    public void GiveMana(float ammount, int resource) {
        if (resource < 0 || resource > _maxResource.Length)
            throw new Exception("Ircorrect manatype");
        _resource[resource] += ammount;
        if (_resource[resource] > _maxResource[resource]) {
            _resource[resource] = _maxResource[resource];
        }
        if (_resource[resource] < 0) {
            _resource[resource] = 0;
        }
    }

    public void SetResource(float ammount, int resourceType) {
        if (resourceType < 0 || resourceType >= 2) {
            Debug.LogError("Ircorrect mana index passed");
            return;
        }
        if (ammount < 0)
            ammount = 0;
        _resource[resourceType] = ammount;
    }

    public void SpendMana(float ammount, int resource) {
        if (resource < 0 || resource >= 2) {
            Debug.LogError("Ircorrect mana index passed");
            return;
        }
        if (_resource[resource] > ammount)
            _resource[resource] -= ammount;
        else
            _resource[resource] = 0;
    }
    #endregion

    #region Stats
    public float AttackSpeed => baseUnit.AttackSpeed;

    public float BlockChance => _totalStats[UnitStats.BLOCK];

    public float EvadeChance => _totalStats[UnitStats.EVADE];

    public float HasteCasttimeModification => 1 + _totalStats[UnitStats.HASTE] / 100;

    public float ParryChance => _totalStats[UnitStats.PARRY];

    public float Spellpower => _totalStats[UnitStats.SPELLPOWER] * _totalStats[UnitStats.SPELLPOWER_PERCENT] / 100;

    private void AddStatusBonuses(Status status) {
        _statusBonuses[status] = StatsTable.FromStatus(status);
        _totalStats += _statusBonuses[status];
    }

    public void UpdateStatusBonuses(Status status) {
        if (status.DeclareFunctions() == null) { return; }
        if (!_statusBonuses.ContainsKey(status)) {
            throw new Exception("Can't update status");
        }
        StatsTable temp = StatsTable.FromStatus(status);
        _totalStats += temp - _statusBonuses[status];
        _statusBonuses[status] = StatsTable.FromStatus(status);

    }

    public void UpdateAllStatusBonuses() {
        foreach (Status status in statusList) {
            UpdateStatusBonuses(status);
        }

    }

    private void RemoveStatusBonuses(Status status) {
        if (status.DeclareFunctions() == null) { return; }
        if (!_statusBonuses.ContainsKey(status)) {
            throw new Exception("Can't update status");
        }
        _totalStats -= _statusBonuses[status];
        _statusBonuses.Remove(status);
    }


    #endregion


    /* TODO: */
    public float Attack => _totalStats.ATK;

    public bool AttackReady => _attackDelay == 0;

    public Unit AggroTarget => target;

    public Vector3 Origin => _location;

    public float Gcd => _gcd;

    public void StartGcd(float duration) { _gcd = duration; }

    public Color GetResourceColor(bool leftColor) {
        return leftColor ? this.leftColor : rightColor;
    }

    public Team Team => team;

    public bool IsDisarmed() {
        foreach (Status s in statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_DISARMED])
                return true;
        }
        return false;
    }

    public bool IsInvisible() {
        foreach (Status s in statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_INVISIBLE])
                return true;
        }
        return false;
    }

    public bool IsRooted() {
        foreach (Status s in statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_ROOTED])
                return true;
        }
        return false;
    }

    public bool IsSilenced() {
        foreach (Status s in statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_SILENCED])
                return true;
        }
        return false;
    }

    public bool IsStunned() {
        foreach (Status s in statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_STUNNED])
                return true;
        }
        return false;
    }

    public void MoveToUnit(Unit unit) {
        target = unit;
    }

    public void MoveToPosition(Vector3 dest) {
        StopCasting(false);
        _moveTo = dest;
        _moving = true;
        GetComponent<Animator>().SetFloat("Speed", 5, 0, Time.deltaTime);
        //agent.SetDestination(dest);
    }

    public void OnPointerUp(PointerEventData eventData) {
        MoveToPosition(Camera.main.ScreenToWorldPoint(eventData.position));
    }

    public void OnPointerDownDelegate(PointerEventData data) {
        Debug.Log("OnPointerDownDelegate called.");
    }

    //Update abilities cooldown, status duration, procces cast and regen resource
    //Backfires UpdateCastEvent
    private void Update() {
        if (UI.Instance.paused)
            return;

        float time = Time.deltaTime;

        foreach (Ability a in abilities) {
            if (a != null) {
                a.Update(time);
            }
        }
        if (!_alive) { return; }
        for (int i = statusList.Count - 1; i >= 0; i--) {
            if (i > 0 && statusList.Count > i)
                statusList[i].Update(time);
        }
        if (_attackDelay > 0)
            _attackDelay = math.max(_attackDelay - time, 0);
        if (_isCasting)
            ProcessCast(time);
        if (_isChannel)
            ProcessChannel(time);

        else if (_attackDelay == 0 && target != null) {
            if (_meele)
                PerformAttack(target, true, true);
            else
                ProjectileManager.CreateTrackingProjectile(Origin, target, _projectileSpeed, 5, true, false, "", null, this, null);
        }

        for (int i = 0; i < baseResourceRegen.Length; i++) {
            GiveMana(baseResourceRegen[i] * time, i);
        }
        if (Debug.isDebugBuild) {
            SetLocation(gameObject.transform.position);
        }
        if (_moving) {

            Quaternion rot = Quaternion.LookRotation(_moveTo - _location);
            float rotAc = 0;
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rot.eulerAngles.y, ref rotAc, _turnRate * time);

            _location = Vector3.MoveTowards(_location, _moveTo, _moveSpeed * time);

            transform.eulerAngles = new Vector3(0, rotationY, 0);

            if (_location == _moveTo) {
                _moving = false;
                GetComponent<Animator>().SetFloat("Speed", 0, 0, 0);
            }
        }
        gameObject.transform.position = _location;

    }

    //Recalculate all Stat bonuses inflicted by statuses


    public void SetLocation(Vector3 location) {
        _location = location;
        gameObject.transform.position = location;
    }

    public void SetTarget(Unit target) {
        this.target = target;
    }

    //Change Team of unit and backfires TeamChangeEvent
    public void SetTeam(Team team) {
        Team last = this.team;
        this.team = team;
        TeamChange?.Invoke(this, last);
    }

    public bool Moving => _moving;

    public void PerformAttack(Unit target, bool useAttackModifiers, bool startCooldown, bool ignorRange = false) {
        if (ignorRange == false && RotW.CheckDistance(target, this, 5)) return;
        RotW.ApplyDamage(new DamageEvent(target, this, Attack, null, 0));
        if (startCooldown)
            _attackDelay = baseUnit.AttackSpeed;
    }

    public void StartAttack(Unit select) {
        target = select;
    }

    //Virtual methods
    public virtual void OnHealDealth() { }

    public void QueueAbility(Ability ability) {
        _queuedAbility = ability;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Controller.Instance.SetSelectedUnit(this);
    }
    //Private methods



}
