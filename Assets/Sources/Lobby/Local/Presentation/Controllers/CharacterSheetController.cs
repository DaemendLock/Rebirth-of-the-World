using Combat.Common.Primitives;

using Lobby.Local.Domain.UseCases.Scenarios;

namespace Lobby.Local.Presentation.Controllers
{
    public sealed class CharacterSheetController
    {
        private ScenarioRequestSelectCharacterUseCase _scenarioSelectCharacterUseCase;

        public CharacterSheetController(ScenarioRequestSelectCharacterUseCase scenarioSelectCharacterUseCase)
        {
            _scenarioSelectCharacterUseCase = scenarioSelectCharacterUseCase;
        }

        public bool Pick(CharacterKey? characterKey) => _scenarioSelectCharacterUseCase.Execute(characterKey);
    }
}
