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

        public static DamageModification operator +(DamageModification value1, DamageModification value2)
        {
            return new(value1.BaseValue + value2.BaseValue,
                        value1.PercentModication + value2.PercentModication,
                        value1.BonusValue + value2.BonusValue,
                        value1.FlagsModification | value2.FlagsModification);
        }
    }
}
