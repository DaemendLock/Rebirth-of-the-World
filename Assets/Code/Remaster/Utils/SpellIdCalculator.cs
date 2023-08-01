namespace Remaster.Utils.SpellIdGenerator
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
        WARRIOR = ((int) ClassType.WEAPON << 2 + (int) ArmorType.HEAVY) << 2 + 0,
        PALADIN = ((int) ClassType.WEAPON << 2 + (int) ArmorType.HEAVY) << 2 + 1,
        SHIELDER = ((int) ClassType.WEAPON << 2 + (int) ArmorType.HEAVY) << 2 + 2,
        DARK_KNIGHT = ((int) ClassType.WEAPON << 2 + (int) ArmorType.HEAVY) << 2 + 3,
        HUNTER = ((int) ClassType.WEAPON << 2 + (int) ArmorType.MEDIUM) << 2 + 0,
        ENCHANTER = ((int) ClassType.WEAPON << 2 + (int) ArmorType.MEDIUM) << 2 + 1,
        SAMURAI = ((int) ClassType.WEAPON << 2 + (int) ArmorType.MEDIUM) << 2 + 2,
        SQUIRE = ((int) ClassType.WEAPON << 2 + (int) ArmorType.LIGHT) << 2 + 0,
        ASSASIN = ((int) ClassType.WEAPON << 2 + (int) ArmorType.LIGHT) << 2 + 1,
        BLADEMASTER = ((int) ClassType.WEAPON << 2 + (int) ArmorType.CLOTHES) << 2 + 0,
        MONK = ((int) ClassType.WEAPON << 2 + (int) ArmorType.CLOTHES) << 2 + 1,
        HEAVY_MAGE = ((int) ClassType.MAGIC << 2 + (int) ArmorType.HEAVY) << 2 + 0,
        RUNE_WRITER = ((int) ClassType.MAGIC << 2 + (int) ArmorType.HEAVY) << 2 + 1,
        BARD = ((int) ClassType.MAGIC << 2 + (int) ArmorType.MEDIUM) << 2 + 0,
        SHAMAN = ((int) ClassType.MAGIC << 2 + (int) ArmorType.MEDIUM) << 2 + 1,
        DRUID = ((int) ClassType.MAGIC << 2 + (int) ArmorType.LIGHT) << 2 + 0,
        AVATAR = ((int) ClassType.MAGIC << 2 + (int) ArmorType.LIGHT) << 2 + 1,
        PRIEST = ((int) ClassType.MAGIC << 2 + (int) ArmorType.CLOTHES) << 2 + 0,
        MAGE = ((int) ClassType.MAGIC << 2 + (int) ArmorType.CLOTHES) << 2 + 1,
        DARK_MAGE = ((int) ClassType.MAGIC << 2 + (int) ArmorType.CLOTHES) << 2 + 2,
        NATURALIST = ((int) ClassType.MAGIC << 2 + (int) ArmorType.CLOTHES) << 2 + 3,
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
        public static int GenerateId(Class @class, Spec spec, int ability)
        {
            return ability << 8 | (int) @class << 2 | (int) spec;
        }
    }
}
