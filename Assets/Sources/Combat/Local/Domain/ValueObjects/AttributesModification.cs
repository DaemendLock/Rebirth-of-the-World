using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly ref struct AttributeModifier
    {
        public readonly float BaseValue;
        public readonly float Percent;
        public readonly float BonusValue;

        public AttributeModifier(AttributeValue value)
        {
            BaseValue = value.BaseValue;
            Percent = value.Percent;
            BonusValue = value.Bonus;
        }

        public AttributeModifier(float baseValue, float percent, float bonusValue)
        {
            BaseValue = baseValue;
            Percent = percent;
            BonusValue = bonusValue;
        }

        public static AttributeModifier operator +(AttributeModifier value1, AttributeModifier value2)
        {
            return new(value1.BaseValue + value2.BaseValue, value1.Percent + value2.Percent, value1.BonusValue + value2.BonusValue);
        }
    }

    public ref struct AttributesModification
    {
        public AttributeModifier Attack { get; set; }

        public AttributeModifier Spellpower { get; set; }

        public AttributeModifier Speed { get; set; }

        public float TimeScale { get; set; }

        public readonly AttributeModifier this[Attribute attribute] => attribute switch
        {
            Attribute.Atk => Attack,
            Attribute.Spellpower => Spellpower,
            Attribute.Speed => Speed,
            _ => default,
        };

        public static AttributesModification operator +(AttributesModification value1, AttributesModification value2)
        {
            AttributesModification result = new();

            result.Attack = value1.Attack + value2.Attack;
            result.Spellpower = value1.Spellpower + value2.Spellpower;
            result.Speed = value1.Speed + value2.Speed;

            return result;
        }
    }
}
