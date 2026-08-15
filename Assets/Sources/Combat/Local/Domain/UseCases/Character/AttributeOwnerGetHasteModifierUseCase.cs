using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct AttributeOwnerGetAttributeValueUseCase
    {
        private readonly IAttributesRepository _attributesRepository;

        public AttributeOwnerGetAttributeValueUseCase(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public float GetAttributeValue(UnitId target, UnitAttribute attributes)
        {
            return _attributesRepository.Get(target)[attributes];
        }
    }

    public readonly struct AttributeOwnerGetVersalityModifierUseCase
    {
        private readonly IAttributesRepository _attributesRepository;

        public AttributeOwnerGetVersalityModifierUseCase(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public float Execute(UnitId target)
        {
            AttributesOwner attributes = _attributesRepository.Get(target);
            return attributes.GetVersalityModifier();
        }
    }

    public readonly struct AttributeOwnerGetHasteModifierUseCase
    {
        private readonly IAttributesRepository _attributesRepository;

        public AttributeOwnerGetHasteModifierUseCase(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public float Execute(UnitId target)
        {
            AttributesOwner attributes = _attributesRepository.Get(target);
            return attributes.GetHasteModifier();
        }
    }
}
