using Game.Application.DTO;
using Game.Application.Outputs;
using Game.Domain.Entities;
using Game.Domain.Repositories;

using Lobby.Common.Primitives;

namespace Game.Application.UseCases.Scenarios
{
    public sealed class ScenarioJoinUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IAccountScenarioJoinOutput _accountScenarioJoinOutput;

        public ScenarioJoinUseCase(IScenarioRepository scenarioRepository, IAccountScenarioJoinOutput accountScenarioJoinOutput)
        {
            _scenarioRepository = scenarioRepository;
            _accountScenarioJoinOutput = accountScenarioJoinOutput;
        }

        public void Execute(ScenarioJoinInfo command)
        {
            AccountId accountId = command.RequestedBy;
            Scenario scenario = _scenarioRepository.Get(command.ScenarioId);
            scenario.Join(accountId);
            _accountScenarioJoinOutput.Present(command.RequestedBy, scenario.Id, scenario._members);
        }
    }
}
