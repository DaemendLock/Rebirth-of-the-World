using Combat.Local.Domain.DTO;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Domain.Facades
{
    public readonly struct SceneFacade
    {
        private readonly CreateCharacterUseCase _createUnitUseCase;
        private readonly ApplyStatusUseCase _applyStatusUseCase;

        public SceneFacade(CreateCharacterUseCase createUnitUseCase, ApplyStatusUseCase applyStatusUseCase)
        {
            _createUnitUseCase = createUnitUseCase;
            _applyStatusUseCase = applyStatusUseCase;
        }

        public void CreateUnit(CreateCharacterDTO dto) => _createUnitUseCase.Execute(dto);

        public void CreateStatus(ApplStatusDTO dto) => _applyStatusUseCase.Execute(dto);
    }
}
