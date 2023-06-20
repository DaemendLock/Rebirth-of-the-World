using System.Collections.Generic;
using UnityEngine;

public enum GearType {
    CLOTHE,
    WEAPON,
    RING,
    TRINKET
}

public enum OldGearSlot : int {
    HEAD,
    BODY,
    LEGS,
    LEFT_ARM,
    RIGHT_ARM,
    RING_1,
    RING_2,
    CONSUMABLE_1,
    CONSUMABLE_2
}

[CreateAssetMenu(fileName = "Unnamed Gear", menuName = "New Gear", order = 56)]
public class OldGear : Item {

    [SerializeField] private GearType _gearType;
    [SerializeField] private OldGearSlot _slot;
    [SerializeField] private List<OldStatsTable.Stat> _stats;

    private OldStatsTable _statsTable = new();

    public GearType GearType => _gearType;

    public OldGearSlot Slot => _slot;

    public OldStatsTable StatsTable => _statsTable;

    public OldGear(GearType gearType) : base(Category.GEAR) {
        _gearType = gearType;
    }

    //private void Awake() { foreach (StatsTable.Stat stat in _stats) _statsTable.SetStat(stat.Type, stat.Value); }

}