using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsTable {
    private Dictionary<UnitStats, float> statsTable = new Dictionary<UnitStats, float>(){
        { UnitStats.ATK, 0 },       //ATK
        { UnitStats.ATK_PERCENT, 0 },
        { UnitStats.SPELLPOWER, 0 },//SPW
        { UnitStats.SPELLPOWER_PERCENT, 0 },
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
        { UnitStats.PARRY, 0 }};

    public float GetStat(UnitStats stat) => statsTable[stat];

    public void SetStat(UnitStats stat, float value) {
        statsTable[stat] = value;
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
        StatsTable res = new StatsTable();

        modifierfunction[] all = status.DeclareFunctions();
        if (all.Contains(modifierfunction.MODIFIER_PROPERTY_ATTACK_BONUS)) { 
            res[UnitStats.ATK] = status.GetModifierAttack_Bonus();
        }
        if (all.Contains(modifierfunction.MODIFIER_PROPERTY_ATTACK_BONUS_PERCENT)) {
            res[UnitStats.ATK_PERCENT] += status.GetModifierAttack_Bonus_Percent();
        }
        if (all.Contains(modifierfunction.MODIFIER_PROPERTY_SPELLPOWER_BONUS)) {
            res[UnitStats.SPELLPOWER] = status.GetModifierSpellpower_Bonus();
        }
        if (all.Contains(modifierfunction.MODIFIER_PROPERTY_SPELLPOWER_BONUS_PERCENT)) {
            res[UnitStats.SPELLPOWER_PERCENT] += status.GetModifierSpellpower_Bonus_Percent();
        }
        if (all.Contains(modifierfunction.MODIFIER_PROPERTY_HASTE_BONUS)) {
            res[UnitStats.HASTE] = status.GetModifierHasteBonus();
        }
        if (all.Contains(modifierfunction.MODIFIER_PROPERTY_HASTE_BONUS_PERCENT)) {
            res[UnitStats.HASTE] += status.GetModifierHasteBonus_Percent();
        }

        return res;
    }

    public static StatsTable operator +(StatsTable table1, StatsTable table2) {
        StatsTable res = new StatsTable();
        foreach (UnitStats stat in table2.statsTable.Keys) {
            res.SetStat(stat, table1[stat] + table2[stat]);
        }
        return res;
    }
    public static StatsTable operator -(StatsTable table1, StatsTable table2) {
        StatsTable res = new StatsTable();
        foreach (UnitStats stat in table2.statsTable.Keys) {
            res.SetStat(stat, Mathf.Max(table1[stat] - table2[stat], 0));
        }
        return res;
    }

    public float ATK => statsTable[UnitStats.ATK];
    public float SPELLPOWER => statsTable[UnitStats.SPELLPOWER];
    public float HASTE => statsTable[UnitStats.HASTE];
    public float CRIT => statsTable[UnitStats.CRIT];
    public float VERSA => statsTable[UnitStats.VERSA];
    public float MAXHP => statsTable[UnitStats.MAXHP];
    public float MAXMANA => statsTable[UnitStats.MAXMANA];
    public float LIFESTEAL => statsTable[UnitStats.LIFESTEAL];
    public float MSPEED => statsTable[UnitStats.MSPEED];
    public float AOERESIST => statsTable[UnitStats.AOERESIST];
    public float BLOCK => statsTable[UnitStats.BLOCK];
    public float EVADE => statsTable[UnitStats.EVADE];
    public float PARRY => statsTable[UnitStats.PARRY];
}

