using Combat.Common.Flags;

namespace Combat.Local.Application.ValueObjects
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

        public readonly float ApplyTo(float damage) =>
            System.Math.Max(0f, (damage + BaseValue) * 100f / (100f + PercentModication) + BonusValue);
    }
}
