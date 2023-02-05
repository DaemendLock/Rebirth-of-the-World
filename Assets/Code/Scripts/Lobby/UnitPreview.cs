using System;
using System.Collections.Generic;
using Data;
using UnityEngine;


public enum UNIT_ROLE {
    NONE,
    TANK,
    HEAL,
    DPS
}

[Serializable]
public class UnitPreview {
    public UnitData baseData;

    //LEFT PART 
    [Header("Left part")]
    public Sprite icon;
    [Header("Gear")]
    public UnitGear[] _gear = new UnitGear[9];



    //RIGHT PART
    [Header ("Right part")]
    public string name;
    public float lvl;
    public float affection;
    public UNIT_ROLE role;
    public List<AbilityData> abilities;
    public StatsTable overallstats;


    public UnitPreview() {

    }

    public UnitPreview(UnitData baseData, Sprite icon, Dictionary<GearSlot, Gear> gear, float lvl, float affection) {
        this.baseData = baseData;
        this.icon = icon;
        if(gear!= null)
        foreach(KeyValuePair<GearSlot, Gear> kvp in gear) {
            _gear[(int)kvp.Key] = new UnitGear(kvp.Value, kvp.Key);
        }
        name = baseData.name;
        this.lvl = lvl;
        this.affection = affection;
        role = baseData.role;
    }

}

public class UnitStatus {
    public readonly float lvl;
    public readonly float affection;
    public readonly float affected;
    public readonly UnitTalents talents;
}

public class UnitTalents {
    public readonly bool[] reserched = new bool[] { false };
    public override string ToString() {
        string res = "";
        for (int i = 0; i < reserched.Length; i += sizeof(char)) {
            res += (reserched[i..(i + sizeof(char))]);
        }
        return res;
    }
}

[Serializable]
public class UnitGear {
    private GearSlot _slot;
    private Gear _item;

    public GearSlot Slot => _slot;
    public Gear GearItem => _item;

    public UnitGear(Gear gear, GearSlot slot) {
        _item = gear;
        _slot = slot;

    }
}