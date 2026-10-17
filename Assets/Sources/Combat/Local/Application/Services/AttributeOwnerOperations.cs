using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.ValueObjects;

namespace Combat.Local.Application.Services.Skills
{
    public sealed class AttributeOwnerOperations
    {
        private readonly IAttributesRepository _attributesRepository;

        public AttributeOwnerOperations(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public void Clear(AttributesOwner attributesOwner)
        {
            attributesOwner = new(attributesOwner.Id, attributesOwner.BaseValues);
            _attributesRepository.Update(attributesOwner);
        }

        public void Cache(AttributesOwner target, AttributesModification finalModification)
        {
            System.ReadOnlySpan<AttributeValue> baseValues = target.BaseValues;
            System.Span<float> values = stackalloc float[baseValues.Length];

            for (int i = 0; i < baseValues.Length; i++)
            {
                AttributeModifier modifier = finalModification[(UnitAttribute)i];
                values[i] = (baseValues[i].BaseValue + modifier.BaseValue) * (baseValues[i].Percent + modifier.Percent) / 100f + modifier.BonusValue;
            }

            _attributesRepository.Update(new(target.Id, baseValues, values));
        }
    }
}
