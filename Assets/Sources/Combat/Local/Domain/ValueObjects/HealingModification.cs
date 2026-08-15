using Combat.Common.Flags;

namespace Combat.Local.Domain.ValueObjects
{
    public ref struct HealingModification
    {
        public float BaseValue;
        public float PercentModication;
        public float BonusValue;
        public HealingFlags FlagsModification;

        public HealingModification(float baseValue, float percentModication, float bonusValue, HealingFlags flagsModification)
        {
            BaseValue = baseValue;
            PercentModication = percentModication;
            BonusValue = bonusValue;
            FlagsModification = flagsModification;
        }

        public static HealingModification operator +(HealingModification value1, HealingModification value2)
        {
            return new(value1.BaseValue + value2.BaseValue,
                        value1.PercentModication + value2.PercentModication,
                        value1.BonusValue + value2.BonusValue,
                        value1.FlagsModification | value2.FlagsModification);
        }
    }
}
