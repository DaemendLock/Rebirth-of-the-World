using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;
using Combat.Local.Domain.ValueObjects;

namespace Testing.Local.Temp.Services
{
    public class AttributeEvaluationService : IAttributeEvaluationService
    {
        private readonly IAttributesRepository _attributeRepository;

        public AttributeEvaluationService(IAttributesRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public float GetAttributeValue(EntityId id, Attribute attribute) => _attributeRepository.Get(id)[attribute].CalculatedValue;
        public float GetHasteModifier(EntityId id) => 1f + GetAttributeValue(id, Attribute.Haste) * 0.007f;
        public float GetVersalityModifier(EntityId id) => 1f + GetAttributeValue(id, Attribute.Versality) * 0.007f;
        public float GetMaxHealthBonus(EntityId id) => GetAttributeValue(id, Attribute.Endurance) * 10f;
    }
}
