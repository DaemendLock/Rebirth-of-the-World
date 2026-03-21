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
    }
}
