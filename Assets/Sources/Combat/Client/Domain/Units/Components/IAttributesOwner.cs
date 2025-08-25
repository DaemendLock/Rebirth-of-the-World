using Client.Combat.Domain.Attributes;

namespace Client.Combat.Domain.Units.Components
{
    public interface IAttributesOwner
    {
        float GetAttributeValue(Attribute attribute);
    }
}
