using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Domain.Facades
{
    public readonly struct AttributeOwnerFacade
    {
        private readonly GetAttributeValueUseCase _getAttributeValueUseCase;
        private readonly GetHasteModifierUseCase _getHasteModifierUseCase;
        private readonly GetVersalityModifierUseCase _getVersalityModifierUseCase;

        public AttributeOwnerFacade(GetHasteModifierUseCase getHasteModifierUseCase, GetVersalityModifierUseCase getVersalityModifierUseCase, GetAttributeValueUseCase getAttributeValueUseCase)
        {
            _getHasteModifierUseCase = getHasteModifierUseCase;
            _getVersalityModifierUseCase = getVersalityModifierUseCase;
            _getAttributeValueUseCase = getAttributeValueUseCase;
        }

        public float GetAttributeValue(EntityId target, Attribute attribute) => _getAttributeValueUseCase.GetAttributeValue(target, attribute);

        public float GetVersalityModifier(EntityId target) => _getVersalityModifierUseCase.Execute(target);

        public float GetHasteModifier(EntityId target) => _getHasteModifierUseCase.Execute(target);
    }
}
