using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Domain.Facades
{
    public readonly struct AttributeOwnerFacade
    {
        private readonly AttributeOwnerGetAttributeValueUseCase _getAttributeValueUseCase;
        private readonly AttributeOwnerGetHasteModifierUseCase _getHasteModifierUseCase;
        private readonly AttributeOwnerGetVersalityModifierUseCase _getVersalityModifierUseCase;

        public AttributeOwnerFacade(AttributeOwnerGetHasteModifierUseCase getHasteModifierUseCase, AttributeOwnerGetVersalityModifierUseCase getVersalityModifierUseCase, AttributeOwnerGetAttributeValueUseCase getAttributeValueUseCase)
        {
            _getHasteModifierUseCase = getHasteModifierUseCase;
            _getVersalityModifierUseCase = getVersalityModifierUseCase;
            _getAttributeValueUseCase = getAttributeValueUseCase;
        }

        public float GetAttributeValue(UnitId target, UnitAttribute attribute) => _getAttributeValueUseCase.GetAttributeValue(target, attribute);

        public float GetVersalityModifier(UnitId target) => _getVersalityModifierUseCase.Execute(target);

        public float GetHasteModifier(UnitId target) => _getHasteModifierUseCase.Execute(target);
    }
}
