using Lobby.Common.Primitives;
using Lobby.Local.Domain.UseCases.Scenarios;

namespace Lobby.Local.Presentation.Controllers
{
    public sealed class CharacterSheetController
    {
        private readonly ScenarioSelectCharacterUseCase _scenarioSelectCharacterUseCase;

        public CharacterSheetController(ScenarioSelectCharacterUseCase scenarioSelectCharacterUseCase)
        {
            _scenarioSelectCharacterUseCase = scenarioSelectCharacterUseCase;
        }

        public bool Select(CharacterKey? character) => _scenarioSelectCharacterUseCase.Execute(character);
    }
}
