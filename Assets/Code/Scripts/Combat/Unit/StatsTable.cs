using System;
using System.Collections.Generic;

public enum UnitStats {
    ATK,
    ATK_PERCENT,
    SPELLPOWER,
    SPELLPOWER_PERCENT,
    HASTE,
    HASTE_PERCENT,
    CRIT,
    CRIT_PERCENT,
    VERSA,
    VERSA_PERCENT,
    MAXHP,
    MAXHP_PERCENT,
    MAXMANA,
    LIFESTEAL,
    LIFESTEAL_PERCENT,
    MSPEED,
    MOVESPEED_PERCENT,
    AOERESIST,
    AOERESIST_PERCENT,
    BLOCK,
    BLOCK_PERCENT,
    EVADE,
    EVADE_PERCENT,
    PARRY,
    PARRY_PERCENT
}

public class StatsTable {

    [Serializable]
    public class Stat {
        public UnitStats Type;
        public float Value;
    }

    private float _atk = 0;
    private float _atkPercent = 0;
    private float _spellpower = 0;
    private float _spellpowerPercent = 0;
    private float _haste = 0;
    private float _hastePercent = 0;
    private float _crit = 0;
    private float _critPercent = 0;
    private float _versality = 0;
    private float _versalityPercent = 0;
    private float _maxHp = 0;
    private float _maxHpPercent = 0;
    private float _lifesteal = 0;
    private float _lifestealPercent = 0;
    private float _movespeed = 0;
    private float _movespeedPercent = 0;
    private float _aoeResist = 0;
    private float _aoeResistPercent = 0;
    private float _block = 0;
    private float _blockPercent = 0;
    private float _evade = 0;
    private float _evadePercent = 0;
    private float _parry = 0;
    private float _parryPercent = 0;

    public static StatsTable EMPTY_TABLE => new();

    #region Properties
    public float Atk { get => _atk; set => _atk = value; }
    public float AtkPercent { get => _atkPercent; set => _atkPercent = value; }
    public float Spellpower { get => _spellpower; set => _spellpower = value; }
    public float SpellpowerPercent { get => _spellpowerPercent; set => _spellpowerPercent = value; }
    public float Haste { get => _haste; set => _haste = value; }
    public float HastePercent { get => _hastePercent; set => _hastePercent = value; }
    public float Crit { get => _crit; set => _crit = value; }
    public float CritPercent { get => _critPercent; set => _critPercent = value; }
    public float Versality { get => _versality; set => _versality = value; }
    public float VersalityPercent { get => _versalityPercent; set => _versalityPercent = value; }
    public float MaxHealth { get => _maxHp; set => _maxHp = value; }
    public float MaxHealthPercent { get => _maxHpPercent; set => _maxHpPercent = value; }
    public float Lifesteal { get => _lifesteal; set => _lifesteal = value; }
    public float LifestealPercent { get => _lifestealPercent; set => _lifestealPercent = value; }
    public float Movespeed { get => _movespeed; set => _movespeed = value; }
    public float MovespeedPercent { get => _movespeedPercent; set => _movespeedPercent = value; }
    public float AoeResist { get => _aoeResist; set => _aoeResist = value; }
    public float AoeResistPercent { get => _aoeResistPercent; set => _aoeResistPercent = value; }
    public float Block { get => _block; set => _block = value; }
    public float BlockPercent { get => _blockPercent; set => _blockPercent = value; }
    public float Evade { get => _evade; set => _evade = value; }
    public float EvadePercent { get => _evadePercent; set => _evadePercent = value; }
    public float Parry { get => _parry; set => _parry = value; }
    public float ParryPercent { get => _parryPercent; set => _parryPercent = value; }
    #endregion

    public float GetStat(UnitStats stat)
        => stat switch {
            UnitStats.ATK => _atk,
            UnitStats.ATK_PERCENT => _atkPercent,
            UnitStats.SPELLPOWER => _spellpower,
            UnitStats.SPELLPOWER_PERCENT => _spellpower,
            UnitStats.HASTE => _haste,
            UnitStats.HASTE_PERCENT => _hastePercent,
            UnitStats.CRIT => _crit,
            UnitStats.CRIT_PERCENT => _crit,
            UnitStats.VERSA => _versality,
            UnitStats.VERSA_PERCENT => _versalityPercent,
            UnitStats.MAXHP => _maxHp,
            UnitStats.MAXHP_PERCENT => _maxHpPercent,
            UnitStats.LIFESTEAL => _lifesteal,
            UnitStats.LIFESTEAL_PERCENT => _lifestealPercent,
            UnitStats.MSPEED => _movespeed,
            UnitStats.MOVESPEED_PERCENT => _movespeedPercent,
            UnitStats.AOERESIST => _aoeResist,
            UnitStats.AOERESIST_PERCENT => _aoeResistPercent,
            UnitStats.BLOCK => _block,
            UnitStats.BLOCK_PERCENT => _blockPercent,
            UnitStats.EVADE => _evade,
            UnitStats.EVADE_PERCENT => _evadePercent,
            UnitStats.PARRY => _parry,
            UnitStats.PARRY_PERCENT => _parryPercent,
            _ => throw new NotImplementedException()
        };

    public void SetStat(UnitStats stat, float value) {
        switch (stat) {
            case UnitStats.ATK:
                _atk = value;
                break;
            case UnitStats.ATK_PERCENT:
                _atkPercent = value;
                break;
            case UnitStats.SPELLPOWER:
                _spellpower = value;
                break;
            case UnitStats.SPELLPOWER_PERCENT:
                _spellpower = value;
                break;
            case UnitStats.HASTE:
                _haste = value;
                break;
            case UnitStats.HASTE_PERCENT:
                _hastePercent = value;
                break;
            case UnitStats.CRIT:
                _crit = value;
                break;
            case UnitStats.CRIT_PERCENT:
                _crit = value;
                break;
            case UnitStats.VERSA:
                _versality = value;
                break;
            case UnitStats.VERSA_PERCENT:
                _versalityPercent = value;
                break;
            case UnitStats.MAXHP:
                _maxHp = value;
                break;
            case UnitStats.MAXHP_PERCENT:
                _maxHpPercent = value;
                break;
            case UnitStats.LIFESTEAL:
                _lifesteal = value;
                break;
            case UnitStats.LIFESTEAL_PERCENT:
                _lifestealPercent = value;
                break;
            case UnitStats.MSPEED:
                _movespeed = value;
                break;
            case UnitStats.MOVESPEED_PERCENT:
                _movespeedPercent = value;
                break;
            case UnitStats.AOERESIST:
                _aoeResist = value;
                break;
            case UnitStats.AOERESIST_PERCENT:
                _aoeResistPercent = value;
                break;
            case UnitStats.BLOCK:
                _block = value;
                break;
            case UnitStats.BLOCK_PERCENT:
                _blockPercent = value;
                break;
            case UnitStats.EVADE:
                _evade = value;
                break;
            case UnitStats.EVADE_PERCENT:
                _evadePercent = value;
                break;
            case UnitStats.PARRY:
                _parry = value;
                break;
            case UnitStats.PARRY_PERCENT:
                _parryPercent = value;
                break;
        }
    }

    public float this[UnitStats stat] {
        get => GetStat(stat);
        set => SetStat(stat, value);
    }

    public static StatsTable FromDictionary(Dictionary<UnitStats, float> dict) {
        StatsTable res = new StatsTable();
        foreach (UnitStats stat in dict.Keys) {
            res.SetStat(stat, dict[stat]);
        }
        return res;
    }

    public static StatsTable FromStatus(Status status) {
        return status.Bonuses;
    }

    public void Clear() {
        _atk = 0;
        _atkPercent = 0;
        _spellpower = 0;
        _spellpowerPercent = 0;
        _haste = 0;
        _hastePercent = 0;
        _crit = 0;
        _critPercent = 0;
        _versality = 0;
        _versalityPercent = 0;
        _maxHp = 0;
        _maxHpPercent = 0;
        _lifesteal = 0;
        _lifestealPercent = 0;
        _movespeed = 0;
        _movespeedPercent = 0;
        _aoeResist = 0;
        _aoeResistPercent = 0;
        _block = 0;
        _blockPercent = 0;
        _evade = 0;
        _evadePercent = 0;
        _parry = 0;
        _parryPercent = 0;
    }

    public static StatsTable operator +(StatsTable table1, StatsTable table2) {
        return new (){
            _atk = table1._atk + table2._atk,
            _atkPercent = table1._atkPercent + table2._atkPercent,
            _spellpower = table1._spellpower + table2._spellpower,
            _spellpowerPercent = table1._spellpowerPercent + table2._spellpowerPercent,
            _haste = table1._haste + table2._haste,
            _hastePercent = table1._hastePercent + table2._hastePercent,
            _crit = table1._crit + table2._crit,
            _critPercent = table1._critPercent + table2._critPercent,
            _versality = table1._versality + table2._versalityPercent,
            _versalityPercent = table1._versalityPercent + table2._versalityPercent,
            _maxHp = table1._maxHp + table2._maxHp,
            _maxHpPercent = table1._maxHpPercent + table2._maxHpPercent,
            _lifesteal = table1._lifesteal + table2._lifesteal,
            _lifestealPercent = table1._lifesteal + table2._lifesteal,
            _movespeed = table1._movespeed + table2._movespeed,
            _movespeedPercent = table1._movespeedPercent + table2._movespeedPercent,
            _aoeResist = table1._aoeResist + table2._aoeResist,
            _aoeResistPercent = table1._aoeResistPercent + table2._aoeResistPercent,
            _block = table1._block + table2._block,
            _blockPercent = table1._blockPercent + table2._blockPercent,
            _evade = table1._evade + table2._evade ,
            _evadePercent = table1._evadePercent + table2._evadePercent,
            _parry = table1._parry + table2._parry,
            _parryPercent = table1._parryPercent + table2._parryPercent,
        };
    }
    
    public static StatsTable operator -(StatsTable table1, StatsTable table2) {
        return new() {
            _atk = table1._atk - table2._atk,
            _atkPercent = table1._atkPercent - table2._atkPercent,
            _spellpower = table1._spellpower - table2._spellpower,
            _spellpowerPercent = table1._spellpowerPercent - table2._spellpowerPercent,
            _haste = table1._haste - table2._haste,
            _hastePercent = table1._hastePercent - table2._hastePercent,
            _crit = table1._crit - table2._crit,
            _critPercent = table1._critPercent - table2._critPercent,
            _versality = table1._versality - table2._versalityPercent,
            _versalityPercent = table1._versalityPercent - table2._versalityPercent,
            _maxHp = table1._maxHp - table2._maxHp,
            _maxHpPercent = table1._maxHpPercent - table2._maxHpPercent,
            _lifesteal = table1._lifesteal - table2._lifesteal,
            _lifestealPercent = table1._lifesteal - table2._lifesteal,
            _movespeed = table1._movespeed - table2._movespeed,
            _movespeedPercent = table1._movespeedPercent - table2._movespeedPercent,
            _aoeResist = table1._aoeResist - table2._aoeResist,
            _aoeResistPercent = table1._aoeResistPercent - table2._aoeResistPercent,
            _block = table1._block - table2._block,
            _blockPercent = table1._blockPercent - table2._blockPercent,
            _evade = table1._evade - table2._evade,
            _evadePercent = table1._evadePercent - table2._evadePercent,
            _parry = table1._parry - table2._parry,
            _parryPercent = table1._parryPercent - table2._parryPercent,
        };
    }



    public static StatsTable Default = new();
}

