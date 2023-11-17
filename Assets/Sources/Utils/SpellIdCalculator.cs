using Utils.DataTypes;

namespace Utils.SpellIdGenerator
{
    public enum ClassType
    {
        WEAPON,
        MAGIC
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
        WARRIOR = (ClassType.WEAPON << 2) + (ArmorType.HEAVY << 2) + 0,
        PALADIN = (ClassType.WEAPON << 2) + (ArmorType.HEAVY << 2) + 1,
        SHIELDER = (ClassType.WEAPON << 2) + (ArmorType.HEAVY << 2) + 2,
        DARK_KNIGHT = (ClassType.WEAPON << 2) + (ArmorType.HEAVY << 2) + 3,
        HUNTER = (ClassType.WEAPON << 2) + (ArmorType.MEDIUM << 2) + 0,
        ENCHANTER = (ClassType.WEAPON << 2) + (ArmorType.MEDIUM << 2) + 1,
        SAMURAI = (ClassType.WEAPON << 2) + (ArmorType.MEDIUM << 2) + 2,
        SQUIRE = (ClassType.WEAPON << 2) + (ArmorType.LIGHT << 2) + 0,
        ASSASIN = (ClassType.WEAPON << 2) + (ArmorType.LIGHT << 2) + 1,
        BLADEMASTER = (ClassType.WEAPON << 2) + (ArmorType.CLOTHES << 2) + 0,
        MONK = (ClassType.WEAPON << 2) + (ArmorType.CLOTHES << 2) + 1,
        HEAVY_MAGE = (ClassType.MAGIC << 2) + (ArmorType.HEAVY << 2) + 0,
        RUNE_WRITER = (ClassType.MAGIC << 2) + (ArmorType.HEAVY << 2) + 1,
        BARD = (ClassType.MAGIC << 2) + (ArmorType.MEDIUM << 2) + 0,
        SHAMAN = (ClassType.MAGIC << 2) + (ArmorType.MEDIUM << 2) + 1,
        DRUID = (ClassType.MAGIC << 2) + (ArmorType.LIGHT << 2) + 0,
        AVATAR = (ClassType.MAGIC << 2) + (ArmorType.LIGHT << 2) + 1,
        PRIEST = (ClassType.MAGIC << 2) + (ArmorType.CLOTHES << 2) + 0,
        MAGE = (ClassType.MAGIC << 2) + (ArmorType.CLOTHES << 2) + 1,
        DARK_MAGE = (ClassType.MAGIC << 2) + (ArmorType.CLOTHES << 2) + 2,
        NATURALIST = (ClassType.MAGIC << 2) + (ArmorType.CLOTHES << 2) + 3,
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
            return (SpellId) ((ability << 8) | ((int) @class << 2) | ((int) spec));
        }
    }
}
