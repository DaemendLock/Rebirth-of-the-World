using Game.Application.DTO;
using Game.Application.Outputs;
using Game.Domain.Entities;
using Game.Domain.Repositories;

using Lobby.Common.Primitives;

namespace Game.Application.UseCases
{
    public sealed class AccountSetAvailableScenariosUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IAccountAvailableScenariosOutput _accountAvailableScenariosOutput;

        public AccountSetAvailableScenariosUseCase(IScenarioRepository scenarioRepository, IAccountAvailableScenariosOutput accountAvailableScenariosOutput)
        {
            _scenarioRepository = scenarioRepository;
            _accountAvailableScenariosOutput = accountAvailableScenariosOutput;
        }

        public void Execute(AccountId accountId, ScenarioId[] scenarios)
        {
            ScenarioInfo[] result = new ScenarioInfo[scenarios.Length];

            for (int i = 0; i < result.Length; i++)
            {
                Scenario value = _scenarioRepository.Get(scenarios[i]);
                result[i] = new(value.Id, value.Name);
            }

            _accountAvailableScenariosOutput.Notify(accountId, result);
        }
    }
}
