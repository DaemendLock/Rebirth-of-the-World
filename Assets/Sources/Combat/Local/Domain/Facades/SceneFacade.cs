using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.UseCases;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Facades
{
    public readonly struct SceneFacade
    {
        private readonly CreateCharacterUseCase _createUnitUseCase;
        private readonly ApplyStatusUseCase _applyStatusUseCase;
        private readonly FindCharactersInRadiusUseCase _findCharacterInRadiusUseCase;

        public SceneFacade(CreateCharacterUseCase createUnitUseCase, ApplyStatusUseCase applyStatusUseCase, FindCharactersInRadiusUseCase findCharacterInRadiusUseCase)
        {
            _createUnitUseCase = createUnitUseCase;
            _applyStatusUseCase = applyStatusUseCase;
            _findCharacterInRadiusUseCase = findCharacterInRadiusUseCase;
        }

        public void CreateUnit(CreateCharacterDTO dto) => _createUnitUseCase.Execute(dto);

        public void CreateStatus(ApplStatusDTO dto) => _applyStatusUseCase.Execute(dto);

        public ICollection<EntityId> FindCharactersInRadius(Vector3 position, float radius) => _findCharacterInRadiusUseCase.Execute(position, radius);
    }
}
