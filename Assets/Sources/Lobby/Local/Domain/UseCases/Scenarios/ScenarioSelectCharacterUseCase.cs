using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioSelectCharacterUseCase
    {
        private readonly LobbySession _session;
        private readonly IScenarioRepository _scenarioRepository;

        public ScenarioSelectCharacterUseCase(IScenarioRepository scenarioRepository, LobbySession session)
        {
            _scenarioRepository = scenarioRepository;
            _session = session;
        }

        public bool Execute(ScenarioId scenarioId, CharacterKey? characterId)
        {
            AccountId accountId = _session.ActiveAccountId;
            Scenario scenario = _scenarioRepository.Get(scenarioId);

            if (TrySelect(scenario, accountId, characterId))
            {
                for (int i = 0; i < scenario.SelectedCharacters.Length; i++)
                {
                    var item = scenario.SelectedCharacters[i];

                    if (item.HasValue == false)
                    {
                        continue;
                    }

                    if (item.Value.Player != accountId)
                    {
                        continue;
                    }

                    scenario.SelectedCharacters[i] = new(accountId, characterId);
                }

                return true;
            }

            return false;
        }

        private bool TrySelect(Scenario scenario, AccountId accountId, CharacterKey? characterId)
        {
            if (characterId.HasValue == false)
            {
                return true;
            }

            foreach (var selection in scenario.SelectedCharacters)
            {
                if (selection.HasValue == false)
                {
                    continue;
                }

                if (selection.Value.CharacterKey == characterId)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
