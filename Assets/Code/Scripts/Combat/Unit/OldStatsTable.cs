using Combat.Status;
using System;
using System.Collections.Generic;

public enum OldUnitStats {
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

public class OldStatsTable {

    [Serializable]
    public class Stat {
        public OldUnitStats Type;
        public float Value;
    }

    public static OldStatsTable EMPTY_TABLE => new();

    #region Properties
    public float Atk { get; set;  }
    public float AtkPercent { get; set; }
    public float Spellpower { get; set; }
    public float SpellpowerPercent { get; set; }
    public float Haste { get; set; }
    public float HastePercent { get; set; }
    public float Crit { get; set; }
    public float CritPercent { get; set; }
    public float Versality { get; set; }
    public float VersalityPercent { get; set; }
    public float MaxHealth { get; set; }
    public float MaxHealthPercent { get; set; }
    public float Lifesteal { get; set; }
    public float LifestealPercent { get; set; }
    public float Movespeed { get; set; }
    public float MovespeedPercent { get; set; }
    public float AoeResist { get; set; }
    public float AoeResistPercent { get; set; }
    public float Block { get; set; }
    public float BlockPercent { get; set; }
    public float Evade { get; set; }
    public float EvadePercent { get; set; }
    public float Parry { get; set; }
    public float ParryPercent { get; set; }
    #endregion

    public float GetStat(OldUnitStats stat)
        => stat switch {
            OldUnitStats.ATK => Atk,
            OldUnitStats.ATK_PERCENT => AtkPercent,
            OldUnitStats.SPELLPOWER => Spellpower,
            OldUnitStats.SPELLPOWER_PERCENT => Spellpower,
            OldUnitStats.HASTE => Haste,
            OldUnitStats.HASTE_PERCENT => HastePercent,
            OldUnitStats.CRIT => Crit,
            OldUnitStats.CRIT_PERCENT => CritPercent,
            OldUnitStats.VERSA => Versality,
            OldUnitStats.VERSA_PERCENT => VersalityPercent,
            OldUnitStats.MAXHP => MaxHealthPercent,
            OldUnitStats.MAXHP_PERCENT => MaxHealthPercent,
            OldUnitStats.LIFESTEAL => Lifesteal,
            OldUnitStats.LIFESTEAL_PERCENT => LifestealPercent,
            OldUnitStats.MSPEED => Movespeed,
            OldUnitStats.MOVESPEED_PERCENT => MovespeedPercent,
            OldUnitStats.AOERESIST => AoeResist,
            OldUnitStats.AOERESIST_PERCENT => AoeResistPercent,
            OldUnitStats.BLOCK => Block,
            OldUnitStats.BLOCK_PERCENT => BlockPercent,
            OldUnitStats.EVADE => Evade,
            OldUnitStats.EVADE_PERCENT => EvadePercent,
            OldUnitStats.PARRY => Parry,
            OldUnitStats.PARRY_PERCENT => ParryPercent,
            _ => throw new NotImplementedException()
        };

    public void SetStat(OldUnitStats stat, float value) {
        switch (stat) {
            case OldUnitStats.ATK:
                Atk = value;
                break;
            case OldUnitStats.ATK_PERCENT:
                AtkPercent = value;
                break;
            case OldUnitStats.SPELLPOWER:
                Spellpower = value;
                break;
            case OldUnitStats.SPELLPOWER_PERCENT:
                Spellpower = value;
                break;
            case OldUnitStats.HASTE:
                Haste = value;
                break;
            case OldUnitStats.HASTE_PERCENT:
                HastePercent = value;
                break;
            case OldUnitStats.CRIT:
                Crit = value;
                break;
            case OldUnitStats.CRIT_PERCENT:
                Crit = value;
                break;
            case OldUnitStats.VERSA:
                Versality = value;
                break;
            case OldUnitStats.VERSA_PERCENT:
                VersalityPercent = value;
                break;
            case OldUnitStats.MAXHP:
                MaxHealth = value;
                break;
            case OldUnitStats.MAXHP_PERCENT:
                MaxHealthPercent = value;
                break;
            case OldUnitStats.LIFESTEAL:
                Lifesteal = value;
                break;
            case OldUnitStats.LIFESTEAL_PERCENT:
                LifestealPercent = value;
                break;
            case OldUnitStats.MSPEED:
                Movespeed = value;
                break;
            case OldUnitStats.MOVESPEED_PERCENT:
                MovespeedPercent = value;
                break;
            case OldUnitStats.AOERESIST:
                AoeResist = value;
                break;
            case OldUnitStats.AOERESIST_PERCENT:
                AoeResistPercent = value;
                break;
            case OldUnitStats.BLOCK:
                Block = value;
                break;
            case OldUnitStats.BLOCK_PERCENT:
                BlockPercent = value;
                break;
            case OldUnitStats.EVADE:
                Evade = value;
                break;
            case OldUnitStats.EVADE_PERCENT:
                EvadePercent = value;
                break;
            case OldUnitStats.PARRY:
                Parry = value;
                break;
            case OldUnitStats.PARRY_PERCENT:
                ParryPercent = value;
                break;
        }
    }

    public float this[OldUnitStats stat] {
        get => GetStat(stat);
        set => SetStat(stat, value);
    }

    public static OldStatsTable FromDictionary(Dictionary<OldUnitStats, float> dict) {
        OldStatsTable res = new OldStatsTable();
        foreach (OldUnitStats stat in dict.Keys) {
            res.SetStat(stat, dict[stat]);
        }
        return res;
    }

    public static OldStatsTable FromStatus(Status status) {
        return status.Bonuses;
    }

    public void Clear() {
        Atk = 0;
        AtkPercent = 0;
        Spellpower = 0;
        SpellpowerPercent = 0;
        Haste = 0;
        HastePercent = 0;
        Crit = 0;
        CritPercent = 0;
        Versality = 0;
        VersalityPercent = 0;
        MaxHealth = 0;
        MaxHealthPercent = 0;
        Lifesteal = 0;
        LifestealPercent = 0;
        Movespeed = 0;
        MovespeedPercent = 0;
        AoeResist = 0;
        AoeResistPercent = 0;
        Block = 0;
        BlockPercent = 0;
        Evade = 0;
        EvadePercent = 0;
        Parry = 0;
        ParryPercent = 0;
    }

    public static OldStatsTable operator +(OldStatsTable table1, OldStatsTable table2) {
        return new (){
            Atk = table1.Atk + table2.Atk,
            AtkPercent = table1.AtkPercent + table2.AtkPercent,
            Spellpower = table1.Spellpower + table2.Spellpower,
            SpellpowerPercent = table1.SpellpowerPercent + table2.SpellpowerPercent,
            Haste = table1.Haste + table2.Haste,
            HastePercent = table1.HastePercent + table2.HastePercent,
            Crit = table1.Crit + table2.Crit,
            CritPercent = table1.CritPercent + table2.CritPercent,
            Versality = table1.Versality + table2.VersalityPercent,
            VersalityPercent = table1.VersalityPercent + table2.VersalityPercent,
            MaxHealth = table1.MaxHealth + table2.MaxHealth,
            MaxHealthPercent = table1.MaxHealthPercent + table2.MaxHealthPercent,
            Lifesteal = table1.Lifesteal + table2.Lifesteal,
            LifestealPercent = table1.Lifesteal + table2.Lifesteal,
            Movespeed = table1.Movespeed + table2.Movespeed,
            MovespeedPercent = table1.MovespeedPercent + table2.MovespeedPercent,
            AoeResist = table1.AoeResist + table2.AoeResist,
            AoeResistPercent = table1.AoeResistPercent + table2.AoeResistPercent,
            Block = table1.Block + table2.Block,
            BlockPercent = table1.BlockPercent + table2.BlockPercent,
            Evade = table1.Evade + table2.Evade ,
            EvadePercent = table1.EvadePercent + table2.EvadePercent,
            Parry = table1.Parry + table2.Parry,
            ParryPercent = table1.ParryPercent + table2.ParryPercent,
        };
    }
    
    public static OldStatsTable operator -(OldStatsTable table1, OldStatsTable table2) {
        return new() {
            Atk = table1.Atk - table2.Atk,
            AtkPercent = table1.AtkPercent - table2.AtkPercent,
            Spellpower = table1.Spellpower - table2.Spellpower,
            SpellpowerPercent = table1.SpellpowerPercent - table2.SpellpowerPercent,
            Haste = table1.Haste - table2.Haste,
            HastePercent = table1.HastePercent - table2.HastePercent,
            Crit = table1.Crit - table2.Crit,
            CritPercent = table1.CritPercent - table2.CritPercent,
            Versality = table1.Versality - table2.VersalityPercent,
            VersalityPercent = table1.VersalityPercent - table2.VersalityPercent,
            MaxHealth = table1.MaxHealth - table2.MaxHealth,
            MaxHealthPercent = table1.MaxHealthPercent - table2.MaxHealthPercent,
            Lifesteal = table1.Lifesteal - table2.Lifesteal,
            LifestealPercent = table1.Lifesteal - table2.Lifesteal,
            Movespeed = table1.Movespeed - table2.Movespeed,
            MovespeedPercent = table1.MovespeedPercent - table2.MovespeedPercent,
            AoeResist = table1.AoeResist - table2.AoeResist,
            AoeResistPercent = table1.AoeResistPercent - table2.AoeResistPercent,
            Block = table1.Block - table2.Block,
            BlockPercent = table1.BlockPercent - table2.BlockPercent,
            Evade = table1.Evade - table2.Evade,
            EvadePercent = table1.EvadePercent - table2.EvadePercent,
            Parry = table1.Parry - table2.Parry,
            ParryPercent = table1.ParryPercent - table2.ParryPercent,
        };
    }

    public static OldStatsTable Default = new();
}

