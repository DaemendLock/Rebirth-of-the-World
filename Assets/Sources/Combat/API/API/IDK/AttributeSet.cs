using Combat.Common.ValueObjects;

namespace Combat.Domain.OldAttributes
{
    public ref struct AttributeSet
    {
        public float Attack { get; set; }
        public float SpellPower { get; set; }

        public float Haste { get; set; }
        public float Lethality { get; set; }
        public float Versality { get; set; }

        public float Speed { get; set; }
        public float Endurance { get; set; }

        public readonly AttributeValue[] ToAttributesArray()
        {
            AttributeValue[] attributeValue = new AttributeValue[(int)Attribute.PARRY + 1];

            attributeValue[(int)Attribute.Atk] = new(Attack, 100);
            attributeValue[(int)Attribute.Spellpower] = new(SpellPower, 100);

            attributeValue[(int)Attribute.Haste] = new(Haste, 100);
            attributeValue[(int)Attribute.Lethality] = new(Lethality, 100);
            attributeValue[(int)Attribute.Versality] = new(Versality, 100);

            attributeValue[(int)Attribute.Speed] = new(Speed, 100);
            attributeValue[(int)Attribute.Endurance] = new(Endurance, 100);

            return attributeValue;
        }
    }
}
