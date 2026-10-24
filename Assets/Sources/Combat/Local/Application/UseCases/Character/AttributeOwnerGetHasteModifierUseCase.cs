using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;

namespace Combat.Local.Application.UseCases
{
    public readonly struct AttributeOwnerGetAttributeValueUseCase
    {
        private readonly IUnitStore _unitStore;

        public AttributeOwnerGetAttributeValueUseCase(IUnitStore units)
        {
            _unitStore = units;
        }

        public float GetAttributeValue(UnitId target, UnitAttribute attributes) => _unitStore.Get(target).Attributes[attributes];
    }

    public readonly struct AttributeOwnerGetVersalityModifierUseCase
    {
        private readonly IUnitStore _unitStore;

        public AttributeOwnerGetVersalityModifierUseCase(IUnitStore units)
        {
            _unitStore = units;
        }

        public float Execute(UnitId target) => _unitStore.Get(target).Attributes.GetVersalityModifier();
    }

    public readonly struct AttributeOwnerGetHasteModifierUseCase
    {
        private readonly IUnitStore _unitStore;

        public AttributeOwnerGetHasteModifierUseCase(IUnitStore units)
        {
            _unitStore = units;
        }

        public float Execute(UnitId target)
        {
            AttributesOwner attributes = _unitStore.Get(target).Attributes;
            return attributes.GetHasteModifier();
        }
    }
}
