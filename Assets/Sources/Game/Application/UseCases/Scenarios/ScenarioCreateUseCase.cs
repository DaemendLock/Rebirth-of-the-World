using Game.Application.Outputs;
using Game.Domain.Entities;
using Game.Domain.Repositories;
using Game.Domain.ValueObjects;

using Lobby.Common.Primitives;

using System;

namespace Game.Application.UseCases.Scenarios
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

            ScenarioMemberInfo?[] playerCharacterSelection = new ScenarioMemberInfo?[maxPlayerCount];

            Scenario scenario = new(new(Guid.NewGuid()), locationName, playerCharacterSelection)
            {
                Name = name
            };

            _scenarioRepository.Add(scenario);
            _scenarioCreateOutput.Present(scenario);
            return scenario.Id;
        }
    }
}
