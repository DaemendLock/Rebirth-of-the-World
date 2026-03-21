using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct GetAttributeValueUseCase
    {
        private readonly IAttributesRepository _attributesRepository;

        public GetAttributeValueUseCase(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public float GetAttributeValue(EntityId target, Attribute attributes)
        {
            return _attributesRepository.Get(target)[attributes];
        }
    }

    public readonly struct GetVersalityModifierUseCase
    {
        private readonly IAttributesRepository _attributesRepository;

        public GetVersalityModifierUseCase(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public float Execute(EntityId target)
        {
            AttributesOwner attributes = _attributesRepository.Get(target);
            return attributes.GetVersalityModifier();
        }
    }

    public readonly struct GetHasteModifierUseCase
    {
        private readonly IAttributesRepository _attributesRepository;

        public GetHasteModifierUseCase(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public float Execute(EntityId target)
        {
            AttributesOwner attributes = _attributesRepository.Get(target);
            return attributes.GetHasteModifier();
        }
    }
}
