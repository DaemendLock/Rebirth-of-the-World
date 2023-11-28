using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Gear
{
    public enum GearType : byte
    {
        HAT,
        CLOTHES,
        PANTS,
        BOOTS,
        JEWELRY,
        WEAPON
    }

    public enum WeaponType : byte
    {
        NONE,
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

    public enum Slot : byte
    {
        HEAD,
        CHEST,
        LEGS,
        FOOT,
        JEWELRY_1,
        JEWELRY_2,
        MAIN_HAND,
        OFF_HAND,
        TWO_HAND,
        CONSUMABLE_1,
        CONSUMABLE_2,
    }

    public class Gear
    {
        public readonly ItemId Id;
        public readonly StatsTable Stats;
        public readonly bool HasSpell;
        public readonly SpellId Spell;

        public Gear(ItemId id, StatsTable stats, bool hasSpell, SpellId spell)
        {
            Id = id;
            Stats = stats;
            HasSpell = hasSpell;
            Spell = spell;
        }
    }
}
