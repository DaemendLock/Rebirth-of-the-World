using Combat.API;
using Combat.Common.Primitives;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Scripting.Adapters
{
    public sealed class UnitNewAdapter
    {
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly CharacterFacade _characterFacade;
        private readonly StatusOwnerApplyUseCase _statusOwnerApplyUseCase;
        private readonly StatusRemoveUseCase _statusRemoveUseCase;

        public UnitNewAdapter(HealthOwnerFacade healthOwnerFacade, CharacterFacade characterFacade, StatusOwnerApplyUseCase statusOwnerApplyUseCase, StatusRemoveUseCase statusRemoveUseCase)
        {
            _healthOwnerFacade = healthOwnerFacade;
            _characterFacade = characterFacade;
            _statusOwnerApplyUseCase = statusOwnerApplyUseCase;
            _statusRemoveUseCase = statusRemoveUseCase;
        }

        public UnitNew Adaptee(UnitId id)
        {
            return new UnitNew(id, _healthOwnerFacade, _characterFacade, _statusOwnerApplyUseCase, _statusRemoveUseCase);
        }
    }

    public sealed class CharacterApiAdapter
    {
        private readonly CharacterFacade _characterFacade;
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly AttributeOwnerFacade _attributeOwnerFacade;

        public CharacterApiAdapter(CharacterFacade characterFacade, HealthOwnerFacade healthFacade, AttributeOwnerFacade attributeOwnerFacade)
        {
            _characterFacade = characterFacade;
            _healthOwnerFacade = healthFacade;
            _attributeOwnerFacade = attributeOwnerFacade;
        }

        public Unit Adaptee(UnitId id)
        {
            OldDomainUnitContext result = new(id,
            _characterFacade,
            _healthOwnerFacade,
            _attributeOwnerFacade);
            return new(result);
        }
    }
}
