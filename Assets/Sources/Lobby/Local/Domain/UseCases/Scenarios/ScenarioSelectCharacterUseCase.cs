using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioSelectCharacterUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;

        public bool Execute(AccountId accountId, CharacterId? characterId)
        {
            ScenarioId scenarioId = new();

            Scenario scenario = _scenarioRepository.Get(scenarioId);

            if (TrySelect(scenario, accountId, characterId))
            {
                return false;
            }

            return false;
        }

        private bool TrySelect(Scenario scenario, AccountId accountId, CharacterId? characterId)
        {
            foreach (var selection in scenario.SelectedCharacters)
            {
                if (selection.HasValue == false)
                {
                    continue;
                }

                if (selection.Value.CharacterId != characterId)
                {
                    continue;
                }

                if (selection.Value.Player != accountId)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
