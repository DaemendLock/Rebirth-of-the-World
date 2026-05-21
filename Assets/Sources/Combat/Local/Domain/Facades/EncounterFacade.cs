using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.UseCases;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Facades
{
    public readonly struct EncounterFacade
    {
        private readonly CharacterCreateUseCase _createUnitUseCase;
        private readonly StatusApplyUseCase _applyStatusUseCase;
        private readonly FindCharactersInRadiusUseCase _findCharacterInRadiusUseCase;

        public EncounterFacade(CharacterCreateUseCase createUnitUseCase, StatusApplyUseCase applyStatusUseCase, FindCharactersInRadiusUseCase findCharacterInRadiusUseCase)
        {
            _createUnitUseCase = createUnitUseCase;
            _applyStatusUseCase = applyStatusUseCase;
            _findCharacterInRadiusUseCase = findCharacterInRadiusUseCase;
        }

        public UnitId CreateUnit(CreateCharacterDTO dto) => _createUnitUseCase.Execute(dto);

        public void CreateStatus(ApplStatusDTO dto) => _applyStatusUseCase.Execute(dto);

        public IReadOnlyCollection<UnitId> FindCharactersInRadius(Vector3 position, float radius) => _findCharacterInRadiusUseCase.Execute(position, radius);
    }
}
