namespace Combat.Common.ValueObjects
{
    public enum ItemTargetSlot
    {
        None = 0,
        MainHand,
        OffHand,
        BothHands,
        Utility
    }

    public enum EquipmentSlotType
    {
        MainHand,
        OffHand,
        Utility1,
        Utility2,
    }

    public enum ItemCostType
    {
        None,
        Consumable,
        Charges
    }
}
