using Combat.API.DTO;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public interface IStatusModifyAttributesCapability
    {
        AttributesModification GetModification();
    }

    public sealed class ModifyAttributesCapability : IStatusModifyAttributesCapability
    {
        private readonly IAttributesModifier _modifier;

        public ModifyAttributesCapability(IAttributesModifier modifier)
        {
            _modifier = modifier;
        }

        public AttributesModification GetModification()
        {
            AttributesModification result = new();
            System.Span<AttributeValue> baseValues = stackalloc AttributeValue[AttributesOwner.AttributeCount];
            System.Span<AttributeValue> bonusValues = stackalloc AttributeValue[baseValues.Length];

            baseValues.Clear();
            bonusValues.Clear();
            _modifier.GetAttributesBonuses(new AttributesData(baseValues, bonusValues));

            result.Attack = new(bonusValues[(int)UnitAttribute.Atk]);
            result.Spellpower = new(bonusValues[(int)UnitAttribute.Spellpower]);
            result.Speed = new(bonusValues[(int)UnitAttribute.Speed]);
            return result;
        }
    }
}
