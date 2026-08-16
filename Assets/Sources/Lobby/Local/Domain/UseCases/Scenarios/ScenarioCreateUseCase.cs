using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.ValueObjects;

using System;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioCreateUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IScenarioCreateOutput _scenarioCreateOutput;

        public ScenarioCreateUseCase(IScenarioRepository scenarioRepository, IScenarioCreateOutput scenarioCreateOutput)
        {
            _scenarioRepository = scenarioRepository;
            _scenarioCreateOutput = scenarioCreateOutput;
        }

        public ScenarioId Execute(string name, string locationName, int maxPlayerCount)
        {
            if (maxPlayerCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxPlayerCount));
            }

            if (string.IsNullOrWhiteSpace(locationName))
            {
                throw new ArgumentException("A scenario location is required.", nameof(locationName));
            }

            PlayerCharacterSelection?[] playerCharacterSelection = new PlayerCharacterSelection?[maxPlayerCount];

            Scenario scenario = new()
            {
                Id = new(Guid.NewGuid()),
                Name = name,
                LocationName = locationName,
                SelectedCharacters = playerCharacterSelection
            };

            _scenarioRepository.Create(scenario);
            _scenarioCreateOutput.Present(scenario);
            return scenario.Id;
        }
    }

    public interface IScenarioCreateOutput
    {
        void Present(Scenario scenario);
    }
}
