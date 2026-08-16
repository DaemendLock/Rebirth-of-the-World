using Global.Local.DTO;

using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Outputs;
using Lobby.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioStartUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IScenarioStartOutput _output;
        private readonly LobbySession _session;

        public ScenarioStartUseCase(IScenarioRepository scenarioRepository, IScenarioStartOutput output, LobbySession session)
        {
            _scenarioRepository = scenarioRepository;
            _output = output;
            _session = session;
        }

        public void Execute()
        {
            Scenario scenario = _scenarioRepository.Get(_session.ActiveScenarioId);
            List<CombatCharacterInfo> characters = new();

            foreach (var selection in scenario.SelectedCharacters)
            {
                if (selection.HasValue == false)
                {
                    continue;
                }

                CharacterKey? characterKey = selection.Value.CharacterKey;

                if (characterKey.HasValue == false)
                {
                    UnityEngine.Debug.LogError($"Failed start: Player {selection.Value.Player} is starting without a selected character.");
                    return;
                }

                characters.Add(new(characterKey.Value.Value, 0));
            }

            _output.Present(new(scenario.LocationName, characters));
        }
    }
}
