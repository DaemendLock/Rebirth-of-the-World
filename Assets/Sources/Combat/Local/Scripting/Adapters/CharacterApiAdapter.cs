using Combat.API;
using Combat.Common.Primitives;
using Combat.Domain.Repositories;
using Combat.Local.Application.Facades;
using Combat.Local.Application.UseCases;

namespace Combat.Local.Scripting.Adapters
{
    public sealed class UnitNewAdapter
    {
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly CharacterFacade _characterFacade;
        private readonly StatusOwnerApplyUseCase _statusOwnerApplyUseCase;
        private readonly StatusOwnerRemoveUseCase _statusRemoveUseCase;
        private readonly IUnitStore _unitStore;

        public UnitNewAdapter(HealthOwnerFacade healthOwnerFacade, CharacterFacade characterFacade, StatusOwnerApplyUseCase statusOwnerApplyUseCase, StatusOwnerRemoveUseCase statusRemoveUseCase, IUnitStore unitStore)
        {
            _healthOwnerFacade = healthOwnerFacade;
            _characterFacade = characterFacade;
            _statusOwnerApplyUseCase = statusOwnerApplyUseCase;
            _statusRemoveUseCase = statusRemoveUseCase;
            _unitStore = unitStore;
        }

        public UnitNew Adaptee(UnitId id)
        {
            if (_unitStore.TryGet(id, out var unit) == false)
            {
                throw new System.Collections.Generic.KeyNotFoundException();
            }

            return new UnitNew(unit, _healthOwnerFacade, _characterFacade, _statusOwnerApplyUseCase, _statusRemoveUseCase);
        }
    }

    public sealed class CharacterApiAdapter
    {
        private readonly IUnitStore _unitStore;
        private readonly CharacterFacade _characterFacade;
        private readonly HealthOwnerFacade _healthOwnerFacade;

        public CharacterApiAdapter(CharacterFacade characterFacade, HealthOwnerFacade healthFacade, IUnitStore unitStore)
        {
            _characterFacade = characterFacade;
            _healthOwnerFacade = healthFacade;
            _unitStore = unitStore;
        }

        public Unit Adaptee(UnitId id)
        {
            if (_unitStore.TryGet(id, out var unit) == false)
            {
                throw new System.Collections.Generic.KeyNotFoundException();
            }

            OldDomainUnitContext result = new(_characterFacade,
            _healthOwnerFacade,
            unit);
            return new(result);
        }
    }
}
