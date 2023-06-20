using Remaster.Stats;

namespace Remaster.Items
{
    public enum GearType
    {
        HAT,
        CLOTHES,
        PANTS,
        BOOTS,
        JEWELRY,
        WEAPON
    }

    public enum WeaponType
    {
        SWORD,
        SHIELD,
        BARRIER,
        SPEAR,
        FIST_WEAPON,
        DAGGER,
        TWO_HANDED_SWORD,
        BOW,
        STAFF,
        CATALYST
    }

    public enum GearSlot
    {
        HEAD,
        CHEST,
        LEGS,
        FOOT,
        JEWELRY_1,
        JEWELRY_2,
        MAIN_HAND,
        OFF_HAND,
        CONSUMABLE_1,
        CONSUMABLE_2,
    }

    public class Item
    {
        
    }

    public class CombatGear : Item
    {
        public readonly GearType GearType;
        public readonly GearSlot Slot;
        public readonly StatsTable Stats;
        public readonly int SpellId;
        public readonly bool RestrictOffhand;

        public CombatGear(GearType gearType, StatsTable stats, int spellId, bool restrictOffhand)
        {
            GearType = gearType;
            Stats = stats;
            SpellId = spellId;
            RestrictOffhand = restrictOffhand;
        }
    }
}
