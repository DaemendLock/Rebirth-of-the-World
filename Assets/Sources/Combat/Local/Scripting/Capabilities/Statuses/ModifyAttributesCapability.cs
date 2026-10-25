using Combat.API.DTO;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.DTO;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public interface IStatusModifyAttributesCapability
    {
        AttributesBonuses GetModification();
    }

    public sealed class ModifyAttributesCapability : IStatusModifyAttributesCapability
    {
        private readonly IAttributesModifier _modifier;

        public ModifyAttributesCapability(IAttributesModifier modifier)
        {
            _modifier = modifier;
        }

        public AttributesBonuses GetModification()
        {
            AttributesBonuses result = new();
            System.Span<AttributeValue> baseValues = stackalloc AttributeValue[AttributesOwner.AttributeCount];
            System.Span<AttributeValue> bonusValues = stackalloc AttributeValue[baseValues.Length];

            baseValues.Clear();
            bonusValues.Clear();
            _modifier.GetAttributesBonuses(new AttributesData(baseValues, bonusValues));

            result.Attack = new(bonusValues[(int)UnitAttribute.Atk]);
            result.Spellpower = new(bonusValues[(int)UnitAttribute.Spellpower]);
            result.Haste = new(baseValues[(int)UnitAttribute.Haste]);
            result.Lethality = new(baseValues[(int)UnitAttribute.Lethality]);
            result.Versatility = new(baseValues[(int)UnitAttribute.Versality]);
            result.Impact = new(baseValues[(int)UnitAttribute.Impact]);
            result.Endurance = new(baseValues[(int)UnitAttribute.Endurance]);
            result.Swiftness = new(bonusValues[(int)UnitAttribute.Swiftness]);
            return result;
        }
    }
}
