using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Scene;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Facades
{
    public readonly struct EncounterFacade
    {
        private readonly CharacterCreateUseCase _createUnitUseCase;
        private readonly StatusOwnerApplyUseCase _applyStatusUseCase;
        private readonly FindCharactersInRadiusUseCase _findCharacterInRadiusUseCase;

        private readonly EncounterEndUseCase _encounterEndUseCase;

        private readonly ICharacterDeleteQueue _characterDeleteQueue;

        public EncounterFacade(CharacterCreateUseCase createUnitUseCase, StatusOwnerApplyUseCase applyStatusUseCase,
                                FindCharactersInRadiusUseCase findCharacterInRadiusUseCase, ICharacterDeleteQueue characterDeleteUseCase,
                                EncounterEndUseCase encounterEndUseCase)
        {
            _createUnitUseCase = createUnitUseCase;
            _applyStatusUseCase = applyStatusUseCase;
            _findCharacterInRadiusUseCase = findCharacterInRadiusUseCase;
            _characterDeleteQueue = characterDeleteUseCase;
            _encounterEndUseCase = encounterEndUseCase;
        }

        public UnitId CreateUnit(CreateCharacterDTO dto) => _createUnitUseCase.Execute(dto);

        public void CreateStatus(ApplStatusDTO dto) => _applyStatusUseCase.Execute(dto);

        public void RemoveUnit(UnitId id) => _characterDeleteQueue.Enqueue(id);

        public void End() => _encounterEndUseCase.Execute();

        public IReadOnlyCollection<UnitId> FindCharactersInRadius(Vector3 position, float radius) => _findCharacterInRadiusUseCase.Execute(position, radius);
    }
}
