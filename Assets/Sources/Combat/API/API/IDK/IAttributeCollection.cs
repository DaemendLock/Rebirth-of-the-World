using Combat.Common.ValueObjects;

using System.Collections.Generic;

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

        public AttributeValue[] ToAttributesArray()
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

    public class LazyAttributeCollection : IAttributeCollection<Attribute>
    {
        private readonly Dictionary<Attribute, AttributeValue> _values;

        public LazyAttributeCollection()
        {
            _values = new();
        }

        public AttributeValue this[Attribute attribute] { get => _values.GetValueOrDefault(attribute, new(0, 100)); set => _values[attribute] = value; }

        public void Add(IAttributeCollection<Attribute> attribute) => throw new System.NotImplementedException();

        public AttributeValue[] ToAttributesArray()
        {
            AttributeValue[] attributeValue = new AttributeValue[13];

            for (int i = 0; i < attributeValue.Length; i++)
            {
                attributeValue[i] = this[(Attribute)i];
            }

            return attributeValue;
        }
    }

    public interface IAttributeCollection<T> where T : struct, System.Enum
    {
        void Add(IAttributeCollection<T> attribute);

        AttributeValue this[T attribute] { get; set; }
    }
}
