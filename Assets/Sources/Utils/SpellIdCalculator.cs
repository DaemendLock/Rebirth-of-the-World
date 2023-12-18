using Utils.DataTypes;

namespace Utils.SpellIdGenerator
{
    public enum ClassType
    {
        WEAPON,
        MAGIC,
        CHARACTER,
        ITEM
    }

    public enum ArmorType
    {
        HEAVY,
        MEDIUM,
        LIGHT,
        CLOTHES
    }

    public enum Class
    {
        WARRIOR = ClassType.WEAPON + (ArmorType.HEAVY << 2) + (0 << 4),
        PALADIN = ClassType.WEAPON + (ArmorType.HEAVY << 2) + (1 << 4),
        SHIELDER = ClassType.WEAPON + (ArmorType.HEAVY << 2) + (2 << 4),
        DARK_KNIGHT = ClassType.WEAPON + (ArmorType.HEAVY << 2) + (3 << 4),
        HUNTER = ClassType.WEAPON + (ArmorType.MEDIUM << 2) + (0 << 4),
        ENCHANTER = ClassType.WEAPON + (ArmorType.MEDIUM << 2) + (1 << 4),
        SAMURAI = ClassType.WEAPON + (ArmorType.MEDIUM << 2) + (2 << 4),
        SQUIRE = ClassType.WEAPON + (ArmorType.LIGHT << 2) + (0 << 4),
        ASSASIN = ClassType.WEAPON + (ArmorType.LIGHT << 2) + (1 << 4),
        BLADEMASTER = ClassType.WEAPON + (ArmorType.CLOTHES << 2) + (0 << 4),
        MONK = ClassType.WEAPON + (ArmorType.CLOTHES << 2) + (1 << 4),
        HEAVY_MAGE = ClassType.MAGIC + (ArmorType.HEAVY << 2) + (0 << 4),
        RUNE_WRITER = ClassType.MAGIC + (ArmorType.HEAVY << 2) + (1 << 4),
        BARD = ClassType.MAGIC + (ArmorType.MEDIUM << 2) + (0 << 4),
        SHAMAN = ClassType.MAGIC + (ArmorType.MEDIUM << 2) + (1 << 4),
        DRUID = ClassType.MAGIC + (ArmorType.LIGHT << 2) + (0 << 4),
        AVATAR = ClassType.MAGIC + (ArmorType.LIGHT << 2) + (1 << 4),
        PRIEST = ClassType.MAGIC + (ArmorType.CLOTHES << 2) + (0 << 4),
        MAGE = ClassType.MAGIC + (ArmorType.CLOTHES << 2) + (1 << 4),
        DARK_MAGE = ClassType.MAGIC + (ArmorType.CLOTHES << 2) + (2 << 4),
        NATURALIST = ClassType.MAGIC + (ArmorType.CLOTHES << 2) + (3 << 4),
    }

    public enum Spec
    {
        SPEC_1,
        SPEC_2,
        SPEC_3,
        SPEC_4,
    }

    public static class SpellIdCalculator
    {
        public static SpellId GenerateId(Class @class, Spec spec, int ability)
        {
            return (SpellId) ((ability << 8) | ((int) @class) | ((int) spec << 6));
        }

        public static SpellId GenerateGearSpellId(ItemId itemId)
        {
            return (SpellId) ((itemId << 8) | ((int) ClassType.ITEM));
        }
    }
}
