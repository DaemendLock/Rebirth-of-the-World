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
    [SerializeField] private List<StatsTable.Stat> _stats;

    private StatsTable _statsTable = new();

    public GearType GearType => _gearType;

    public GearSlot Slot => _slot;

    public StatsTable StatsTable => _statsTable;

    public Gear(GearType gearType) : base(Category.GEAR) {
        _gearType = gearType;
    }

    //private void Awake() { foreach (StatsTable.Stat stat in _stats) _statsTable.SetStat(stat.Type, stat.Value); }

}