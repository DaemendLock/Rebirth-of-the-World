using Combat.Common.Flags;

namespace Combat.Local.Domain.ValueObjects
{
    public ref struct DamageModification
    {
        public float BaseValue;
        public float PercentModication;
        public float BonusValue;
        public DamageFlags FlagsModification;

        public DamageModification(float baseValue, float percentModication, float bonusValue, DamageFlags flagsModification)
        {
            BaseValue = baseValue;
            PercentModication = percentModication;
            BonusValue = bonusValue;
            FlagsModification = flagsModification;
        }
    }
}
