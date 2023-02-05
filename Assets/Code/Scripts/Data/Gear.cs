using System.Collections.Generic;
using UnityEngine;

public enum GearType {
    CLOTHE,
    WEAPON,
    RING,
    TRINKET
}

public enum GearSlot : int {
    HEAD,
    BODY,
    FEETS,
    LEFT_ARM,
    RIGHT_ARM,
    RING_1,
    RING_2,
    CONSUMABLE_1,
    CONSUMABLE_2
}

[CreateAssetMenu(fileName = "Unnamed Gear", menuName = "New Gear", order = 56)]
public class Gear : Item {

    [SerializeField] private GearType _gearType;
    [SerializeField] private GearSlot _slot;
    [SerializeField] private StatsTable _stats;

    public GearType GearType => _gearType;

    public GearSlot Slot => _slot;

    public Gear(GearType gearType) : base(Type.GEAR) {
        _gearType = gearType;
    }

}