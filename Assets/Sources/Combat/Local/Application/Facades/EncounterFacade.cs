using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.UseCases;
using Combat.Local.Application.UseCases.Scene;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Application.Facades
{
    public readonly struct EncounterFacade
    {
        private readonly UnitCreateUseCase _createUnitUseCase;
        private readonly StatusOwnerApplyUseCase _applyStatusUseCase;
        private readonly FindCharactersInRadiusUseCase _findCharacterInRadiusUseCase;

        private readonly EncounterFinalizeUseCase _encounterEndUseCase;

        private readonly ICharacterDeleteQueue _characterDeleteQueue;

        public EncounterFacade(UnitCreateUseCase createUnitUseCase, StatusOwnerApplyUseCase applyStatusUseCase,
                                FindCharactersInRadiusUseCase findCharacterInRadiusUseCase, ICharacterDeleteQueue characterDeleteUseCase,
                                EncounterFinalizeUseCase encounterEndUseCase)
        {
            _createUnitUseCase = createUnitUseCase;
            _applyStatusUseCase = applyStatusUseCase;
            _findCharacterInRadiusUseCase = findCharacterInRadiusUseCase;
            _characterDeleteQueue = characterDeleteUseCase;
            _encounterEndUseCase = encounterEndUseCase;
        }

        public UnitId CreateUnit(UnitCreationInfo dto) => _createUnitUseCase.Execute(dto);

        public void CreateStatus(ApplyStatusInfo dto) => _applyStatusUseCase.Execute(dto);

        public void RemoveUnit(UnitId id) => _characterDeleteQueue.Enqueue(id);

        public void Finalize(EncounterState reason) => _encounterEndUseCase.Execute(reason);

        public IReadOnlyCollection<UnitId> FindCharactersInRadius(Vector3 position, float radius) => _findCharacterInRadiusUseCase.Execute(position, radius);
    }
}
