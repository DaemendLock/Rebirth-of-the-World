using Data;
using Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitOperations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Team {
    TEAM_ALLY,
    TEAM_ENEMY,
    TEAM_NOTEAM,
}

public abstract class Unit : MonoBehaviour, IPointerClickHandler, IHealthOwner, ICaster {
    private const int PERCENT_TO_MULTIPLIER = 100;
    public const float HASTE_TO_TIME_DIVIDER = 90f;

    //Table to maintain all existing Units. Require for CreateUnit(<name>);
    public static Dictionary<string, Type> allUnitTypes = new();

    //Events declaration
    #region Events
    //Movement Events
    public event Action<Vector3> Moved;

    //Health events:
    public event Action<HealthChangeEventInstance> HealthChanged;
    public event Action<AttackEventInstance> RecivedDamage;
    public event Action<HealEventInstance> Healed;
    public event Action Died;
    public event Action<float> Resurected;

    //Fired on team Changed
    public event Action<Unit, Team> TeamChanged;

    //Cast events:
    public event Action<Ability> AbilityUsed;
    public event Action<Ability> CastStarted;
    public event Action<float> CastProcessed;
    public event Action<Ability, bool> CastEnded;
    public event Action<Ability> ChannelStarted;
    public event Action<float> ChannelProcessed;
    public event Action<Ability, bool> ChannelEnded;

    //Status events:
    public event Action<int, float> ResourceChanged;
    public event Action<Status> StatusApplied;
    public event Action<Status> StatusRemoved;
    #endregion

    #region Variables
    [SerializeField] private ushort _unitId;
    [SerializeField] private UnitTemplate _template;


    //Key Values
    private readonly float[] _baseResourceRegen = new float[2];

    private readonly string _unitLabel;
    private readonly float _turnRate;

    //ColorData
    public Color LeftColor { get; protected set; } = new Color(0, 128, 255);
    public Color RightColor { get; protected set; } = new Color(127, 0, 255);

    //Positioning
    private Vector3 _location = Vector3.zero;
    private float _ringRadius = 0.5f;
    private Vector3 _moveTo = Vector3.zero;
    private bool _moving = false;

    //Define Stat Tables
    protected abstract UnitData BaseUnit { get; }
    private StatsTable _baseStats = StatsTable.EMPTY_TABLE;
    private StatsTable _equipBonus = new();
    private StatsTable _totalStats = new() {
        AtkPercent = 100,
        SpellpowerPercent = 100,
        CritPercent = 100,
        VersalityPercent = 100,
        HastePercent = 100,
        MaxHealthPercent = 100,
        LifestealPercent = 100,
        MovespeedPercent = 100,
        AoeResistPercent = 100,
        BlockPercent = 100,
        EvadePercent = 100,
        ParryPercent = 100,
    };

    //AI
    private AIModule ai;

    [SerializeReference] private List<Ability> _abilities = new(6);
    private Dictionary<GearSlot, Gear> _gear = new Dictionary<GearSlot, Gear> {
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
    [SerializeField] private Team _team;
    private bool _alive = true;
    private float _hp = 0;
    private Overshield _overshield = new();
    private readonly float[] _resource = new float[2];

    //Resources
    private float _minHp;

    //Casting
    private Ability _cast;
    private bool _isCasting;
    private bool _isChannel;
    private float _castTime;
    private float _channelTime;
    private float _gcd;
    private Ability _queuedAbility;
    private readonly float[] _maxResource = new float[2];

    //Flags
    private bool _meele;

    //Velocity
    private float _attackDelay;
    private float _projectileSpeed;

    private Unit _target;

    public float RingRadius => _ringRadius;

    public bool AttackReady => _attackDelay == 0;

    public Unit AggroTarget => _target;

    public Vector3 Origin => _location;

    public float Gcd => _gcd;

    public Team Team => _team;

    public bool Moving => _moving;

    public Vector3 Destination => _moveTo;
    #endregion


    #region Init
    public Unit() {

    }

    protected virtual void Precache() {

    }

    public abstract void Init();

    private void Start() {
        Precache();
        EvaluateBaseUnitStats();
        Init();
        _hp = MaxHealth;
    }
    #endregion

    #region Statuses
    private LinkedList<Status> _statusList = new();

    public List<Status> AllStatuses => new List<Status>(_statusList);

    public void AddNewStatus(Unit caster, Ability ability, string name, Dictionary<string, float> data) {
        Type type = Status.allStatusList.GetValueOrDefault(name, null);
        if (type == null) {
            Debug.LogError("No such Status :" + name);
            return;
        }
        Status check = FindStatusByNameAndCaster(name, caster);
        if (check == null) {
            check = (Status) Activator.CreateInstance(type, this, caster, ability, data);
            _statusList.AddLast(check);
            AddStatusBonuses(check);
            StatusApplied?.Invoke(check);
        } else {
            check.ForceRefresh(data);
        }
    }

    public void AddStatus(Status newStatus) {
        _statusList.AddLast(newStatus);
        AddStatusBonuses(newStatus);
        StatusApplied?.Invoke(newStatus);
    }

    public void AddNewOvershieldStatus(Unit caster, Ability ability, string name, Dictionary<string, float> data, float durability) {
        Type type = Status.allStatusList.GetValueOrDefault(name, null);
        if (type == null) {
            Debug.LogError("No such Status :" + name);
            return;
        }
        Status check = FindStatusByNameAndCaster(name, caster);
        if (check is OvershieldStatus status) {
            if (check == null) {
                check = (OvershieldStatus) Activator.CreateInstance(type, this, caster, ability, data);
                _statusList.AddLast(check);
                AddStatusBonuses(check);
                _overshield.AddStatus((OvershieldStatus) check);
                StatusApplied?.Invoke(check);
            } else {
                ((OvershieldStatus) check).ForceRefresh(durability, data);
            }
        } else
            throw new Exception("Unable to add non-shield status as shield");
    }

    public Status FindStatusByName(string name) {
        Type type = Status.allStatusList[name];
        foreach (Status s in _statusList) {
            if (s.GetType() == type)
                return s;
        }
        return null;
    }

    public Status FindStatusByNameAndCaster(string name, Unit caster) {
        Type type = Status.allStatusList[name];
        foreach (Status s in _statusList) {
            if (s.GetType() == type && s.Caster == caster) { return s; } else if (s.GetType() == type)
                Debug.Log(s.Caster == caster);
        }
        return null;
    }

    public List<Status> FindAllStatusesByName(string name) { return _statusList.Where(s => s.Name.Equals(name)).ToList(); }

    public bool HasStatus(Status status) {
        Type t = status.GetType();
        foreach (Status s in _statusList) {
            if (s.GetType() == t)
                return true;
        }
        return false;
    }

    public bool HasStatus(string status_name) {
        Type t;
        if (Status.allStatusList.TryGetValue(status_name, out t))
            return false;

        foreach (Status s in _statusList) {
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
        if (!_statusList.Contains(s))
            return;
        RemoveStatusBonuses(s);
        _statusList.Remove(s);
    }

    private void UpdateStatuses(float time) {
        for (LinkedListNode<Status> node = _statusList.First; node != null; node = node.Next)
            node.Value.Update(time);
    }
    #endregion

    #region Casting
    public float CastTimeRemain => _castTime;

    public float ChannelTime => _channelTime;

    public bool Casting => _isCasting;

    public bool Channeling => _isChannel;

    public Ability CurrentCastAbility => _cast;

    public Ability QueuedAbility { get => _queuedAbility; }

    public void AddAbility(Ability ability) {
        if (ability.Owner != this)
            throw new Exception($"Can assign ability with Owner {ability.Owner} to {this}");
        _abilities.Add(ability);
    }

    public void CastAbility(Ability ability) {
        if (!ability.CastAbility())
            return;
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
    public Ability GetAbilityByIndex(int index) => index >= _abilities.Count ? null : _abilities[index];

    public void Interrupt(bool succes = false) {
        if (_isCasting)
            StopCasting(succes);
        if (_isChannel)
            StopChannel(succes);
    }

    public void RefreshCD() {
        foreach (Ability ability in _abilities)
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
        _castTime = ability.CastTime / HasteCasttimeDivider;
        ability.OnCastStart();
        CastStarted?.Invoke(ability);
    }

    private void StopCasting(bool succes) {
        _isCasting = false;
        _target = null;
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

        if (_cast != null && _channelTime >= _cast.ChannelTime) {
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
    public float HealthPercent => _hp / MaxHealth;

    public float CurrentHealth => _hp;

    public float MaxHealth => _totalStats.MaxHealth;

    public float MinHealth => _minHp;

    public void SetHealth(float health, Unit inflictor = null, Ability ability = null) {
        float healthWas = _hp;
        _hp = health;
        if (_hp < _minHp)
            _hp = _minHp;
        if (_hp > MaxHealth)
            _hp = MaxHealth;
        HealthChanged?.Invoke(new HealthChangeEventInstance(inflictor, this, ability, _hp, healthWas));
    }

    //Killable
    public bool Alive => _alive;

    public bool Dead => !_alive;

    public void ForceKill(bool resurectable) {
        if (Dead) { throw new Exception("Can't kill dead unit"); }
        for (LinkedListNode<Status> node = _statusList.First; node != null; node = node.Next)
            if (node.Value.RemoveOnDeath())
                _statusList.Remove(node.Value);
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
        ResurectWithHealth(MaxHealth);
    }

    public void ResurectWithHealthPercent(float percent) {
        ResurectWithHealth(MaxHealth * percent * 0.01f);
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
        foreach (Status s in _statusList) {
            if (s.AffectsDamageRecived)
                res += s.GetDamageRecivePercentBonus(e);
        }
        return res;
    }

    public void Damage(AttackEventInstance e) {
        if (!((e.DamageFlags & (DamageFlags.BYPASS_EVADE | DamageFlags.DOT_EFFECT | DamageFlags.HPLOSS)) == 0) && RotW.RollPercentage(EvadeChance)) {
            e.FailType = attackfail.MISS;
            return;
        }
        if (!((e.DamageFlags & (DamageFlags.BYPASS_PARRY | DamageFlags.DOT_EFFECT | DamageFlags.HPLOSS)) == 0) && RotW.RollPercentage(ParryChance)) {
            e.Value = 0;
            e.FailType = attackfail.PARRY;
            RecivedDamage?.Invoke(e);
            return;
        }

        foreach (Status s in _statusList) {
            if (s.AffectsDamageRecived) {
                e.Value -= s.GetBlockConstant(e);
                if (e.Value < 0) {
                    RecivedDamage?.Invoke(e);
                    return;
                }
            }
        }
        e.Value = e.Value * GetDamageReceive(e) * 0.01f;
        if (!((e.DamageFlags & DamageFlags.BYPASS_BLOCK) == 0) && RotW.RollPercentage(BlockChance)) {
            e.Value *= RotW.DAMAGE_BYPASS_BLOCK;
        }
        _overshield.Damage(e);
        if (_hp - e.Value <= 0 && (e.DamageFlags & DamageFlags.NON_LETHAL) != 0) {
            e.Value = _hp - 1;
        }
        _hp -= e.Value;
        RecivedDamage?.Invoke(e);

        if (_hp <= 0) {
            Kill();
        }
    }

    public virtual void OnDamage(AttackEventInstance e) { }

    //Healable
    public void Heal(HealEventInstance e) {
        if (e.Value < 0)
            return;
        float healthWas = _hp;
        e.Value *= GetHealRecive();
        _hp += e.Value;
        if (_hp > MaxHealth) {
            _hp = MaxHealth;
        }

        Healed?.Invoke(e);
        HealthChanged?.Invoke(e);
    }

    public float GetHealRecive() => 100;

    public virtual void OnHealRecived(HealEventInstance e) { }
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
        float oldValue = _resource[resource];
        _resource[resource] += ammount;
        if (_resource[resource] > _maxResource[resource]) {
            _resource[resource] = _maxResource[resource];
        }
        if (_resource[resource] < 0) {
            _resource[resource] = 0;
        }
        ResourceChanged?.Invoke(resource, oldValue);
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
    public float AttackSpeed => BaseUnit.AttackSpeed;

    public float BlockChance => _totalStats.Block * _totalStats.BlockPercent / PERCENT_TO_MULTIPLIER;

    public float EvadeChance => _totalStats.Evade * _totalStats.EvadePercent / PERCENT_TO_MULTIPLIER;

    public float HastePercent => _totalStats.Haste * _totalStats.HastePercent / HASTE_TO_TIME_DIVIDER;

    public float HasteCasttimeDivider => Math.Max(HastePercent, 10f);

    public float ParryChance => _totalStats.Parry * _totalStats.ParryPercent / PERCENT_TO_MULTIPLIER;

    public float Spellpower => _totalStats[UnitStats.SPELLPOWER] * _totalStats[UnitStats.SPELLPOWER_PERCENT] / PERCENT_TO_MULTIPLIER;

    private void AddStatusBonuses(Status status) {
        _totalStats += status.Bonuses;
    }

    private void RemoveStatusBonuses(Status status) {
        _totalStats -= status.Bonuses;
    }

    private void EvaluateBaseUnitStats() {
        SetLevelAndAffinity(0, 0);
        _meele = !BaseUnit.RangedAttack;
        _projectileSpeed = BaseUnit.ProjectileSpeed;
        _maxResource[0] = BaseUnit.MaxResource[0];
        _maxResource[1] = BaseUnit.MaxResource[1];
        _resource[0] = _maxResource[0];
        _resource[1] = _maxResource[1];
        _baseResourceRegen[0] = BaseUnit.ResourceRegen[0];
        _baseResourceRegen[1] = BaseUnit.ResourceRegen[1];
    }

    public void EquipWith(UnitGear gear) {
        if (gear == null)
            return;
        if(_gear[gear.Slot] != null) {
            _equipBonus -= _gear[gear.Slot].StatsTable;
            _totalStats -= _gear[gear.Slot].StatsTable;
        }
        _gear[gear.Slot] = gear.GearItem;
        _equipBonus += gear.GearItem.StatsTable;
        _totalStats += _gear[gear.Slot].StatsTable;
    }

    public void EquipWithAll(UnitGear[] gear) {
        foreach (UnitGear item in gear) {
            EquipWith(item);
        }
    }

    public void SetLevelAndAffinity(byte lvl, byte affinity) {
        _baseStats = BaseUnit.CalculateUnitStatsByRankAndLevel(lvl, affinity);
        _totalStats += _baseStats;
    }

    public float Attack => _totalStats.Atk;
    #endregion

    public void StartGcd(float duration) { _gcd = duration; }

    public Color GetResourceColor(bool leftColor) {
        return leftColor ? LeftColor : RightColor;
    }

    public bool IsDisarmed() {
        foreach (Status s in _statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_DISARMED])
                return true;
        }
        return false;
    }

    public bool IsInvisible() {
        foreach (Status s in _statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_INVISIBLE])
                return true;
        }
        return false;
    }

    public bool IsRooted() {
        foreach (Status s in _statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_ROOTED])
                return true;
        }
        return false;
    }

    public bool IsSilenced() {
        foreach (Status s in _statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_SILENCED])
                return true;
        }
        return false;
    }

    public bool IsStunned() {
        foreach (Status s in _statusList) {
            if (s.CheckState()[modifierstate.MODIFIER_STATE_STUNNED])
                return true;
        }
        return false;
    }

    public void MoveToPosition(Vector3 destination) {
        Interrupt(false);
        destination.y = _location.y;
        _moveTo = destination;
        _moving = true;
        //GetComponent<Animator>().SetFloat("Speed", 5, 0, Time.deltaTime);
        //agent.SetDestination(dest);
    }

    //Update abilities cooldown, status duration, procces cast and regen resource
    //Backfires UpdateCastEvent
    private void Update() {
        if (UI.Instance.paused)
            return;
        float time = Time.deltaTime;

        foreach (Ability a in _abilities) {
            if (a != null) {
                a.Update(time);
            }
        }

        if (!_alive) { return; }

        for (int i = 0; i < _baseResourceRegen.Length; i++) {
            GiveMana(_baseResourceRegen[i] * time, i);
        }

        UpdateStatuses(time);
        if (_attackDelay > 0) {
            _attackDelay = math.max(_attackDelay - time, 0);
        }
        if (_isCasting) {
            ProcessCast(time);
        } else if (_isChannel) {
            ProcessChannel(time);
        } else if (_attackDelay == 0 && _target != null) {
            if (RotW.CheckDistance(this, _target, BaseUnit.AttackRange)) {
                if (_meele)
                    PerformAttack(_target, true, true);
                else
                    ProjectileManager.CreateTrackingProjectile(Origin, _target, _projectileSpeed, 5, true, false, "", null, this, null);
            }
        }
        if (_moving) {
            StepToward(_moveTo, time);
        }
    }

    //Recalculate all Stat bonuses inflicted by statuses
    public void SetLocation(Vector3 location) {
        _location = location;
        transform.position = _location;
    }

    private void StepToward(Vector3 destination, float time) {
        Quaternion rot = Quaternion.LookRotation(destination - _location);
        float rotAc = 0;
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rot.eulerAngles.y, ref rotAc, time);

        _location = Vector3.MoveTowards(_location, destination, _totalStats.Movespeed * time);

        transform.position = _location;
        transform.eulerAngles = new Vector3(0, rotationY, 0);

        if (_location == destination) {
            _moving = false;
        }
    }

    private void StepToward(Unit target, float time) {
        StepToward(target._location, time);
    }

    private void StepToAttack(Unit target, float time) {
        StepToward(target._location, time);
        if (_moving && RotW.CheckDistance(this, target, BaseUnit.AttackRange)) {
            _moving = false;
        }
    }


    public void SetTarget(Unit target) {
        _target = target;
    }

    //Change Team of unit and backfires TeamChangeEvent
    public void SetTeam(Team team) {
        Team last = _team;
        _team = team;
        TeamChanged?.Invoke(this, last);
    }

    public void PerformAttack(Unit target, bool useAttackModifiers, bool startCooldown, bool ignorRange = false) {
        if (ignorRange == false && RotW.CheckDistance(target, this, BaseUnit.AttackRange))
            return;
        RotW.ApplyDamage(new DamageEvent(target, this, Attack, null, 0));
        if (startCooldown)
            _attackDelay = BaseUnit.AttackSpeed;
    }

    public void StartAttack(Unit select) {
        _target = select;
    }

    public void QueueAbility(Ability ability) {
        _queuedAbility = ability;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Controller.Instance.HandleUnitInteraction(this, eventData);
    }
    //Private methods

    private class Overshield {
        private LinkedList<OvershieldStatus> _statuses = new();

        private float _durability = 0;

        public void AddStatus(OvershieldStatus newShield) {
            _durability += newShield.Durability;
            newShield.DurabilityUpdated += Refreshed;
            newShield.ShieldBroke += RemoveStatus;
        }

        public void Damage(AttackEventInstance e) {
            foreach (OvershieldStatus s in _statuses) {
                if (s.Damage(e)) {
                    return;
                }
            }
        }

        private void Refreshed(float durabilityChange) {
            _durability += durabilityChange;
        }

        public void RemoveStatus(OvershieldStatus s) {
            s.ShieldBroke -= RemoveStatus;
            s.DurabilityUpdated -= Refreshed;
            _statuses.Remove(s);
        }
    }

}
