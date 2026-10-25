using Combat.Common.ValueObjects;

namespace Combat.Domain.OldAttributes
{
    public interface IAttribute
    {
        public UnitAttribute Value { get; }
    }

    public readonly struct AtkAttributeType : IAttribute
    {
        public UnitAttribute Value => UnitAttribute.Atk;
    }

    public ref struct NewAttributesSet
    {

    }

    public ref struct AttributeSet
    {
        public float Attack { get; set; }
        public float SpellPower { get; set; }

        public float Haste { get; set; }
        public float Lethality { get; set; }
        public float Versality { get; set; }
        public float Impact { get; set; }

        public float Swiftness { get; set; }
        public float Endurance { get; set; }

        public readonly AttributeValue[] ToAttributesArray()
        {
            AttributeValue[] attributeValue = new AttributeValue[(int)UnitAttribute.Endurance + 1];

            attributeValue[(int)UnitAttribute.Atk] = new(Attack, 100);
            attributeValue[(int)UnitAttribute.Spellpower] = new(SpellPower, 100);

            attributeValue[(int)UnitAttribute.Haste] = new(Haste, 100);
            attributeValue[(int)UnitAttribute.Lethality] = new(Lethality, 100);
            attributeValue[(int)UnitAttribute.Versality] = new(Versality, 100);

            attributeValue[(int)UnitAttribute.Swiftness] = new(Swiftness, 100);
            attributeValue[(int)UnitAttribute.Endurance] = new(Endurance, 100);

            return attributeValue;
        }
    }
}
