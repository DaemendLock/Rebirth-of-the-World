using Global.Local.DTO;

using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Outputs;
using Lobby.Local.Domain.Repositories;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioStartUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IScenarioStartOutput _output;

        public ScenarioStartUseCase(
            IScenarioRepository scenarioRepository,
            IScenarioStartOutput output)
        {
            _scenarioRepository = scenarioRepository;
            _output = output;
        }

        public void Execute(ScenarioId scenarioId)
        {
            Scenario scenario = _scenarioRepository.Get(scenarioId);

            foreach (var selection in scenario.SelectedCharacters)
            {
                if (selection.HasValue == false)
                {
                    continue;
                }

                if (selection.Value.CharacterId.HasValue)
                {
                    continue;
                }

                UnityEngine.Debug.LogWarning($"Player {selection.Value.Player} is starting without a selected character.");
            }

            _output.Present(new StartCombatRequest(scenario.LocationName));
        }
    }
}
