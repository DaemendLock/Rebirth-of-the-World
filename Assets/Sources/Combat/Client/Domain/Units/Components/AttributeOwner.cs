using Client.Combat.Domain.Attributes;

namespace Client.Combat.Domain.Units.Components
{
    public class AttributeOwner : IAttributesOwner
    {
        private readonly IAttributeCollection<Attribute> _attributes;

        public AttributeOwner(IAttributeCollection<Attribute> attributes)
        {
            _attributes = attributes;
        }

        public float GetAttributeValue(Attribute attribute) => _attributes[attribute].CalculatedValue;
    }
}
