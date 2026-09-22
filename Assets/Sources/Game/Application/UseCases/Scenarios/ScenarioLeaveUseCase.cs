using Game.Application.Repositories;
using Game.Domain.Entities;
using Game.Domain.Repositories;

using Lobby.Common.Primitives;

namespace Game.Application.UseCases.Scenarios
{
    public sealed class ScenarioLeaveUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IScenarioQueryGateway _scenarioQuery;

        public ScenarioLeaveUseCase(IScenarioRepository scenarioRepository, IScenarioQueryGateway scenarioQuery)
        {
            _scenarioRepository = scenarioRepository;
            _scenarioQuery = scenarioQuery;
        }

        public void Execute(AccountId requestedBy)
        {
            ScenarioId? scenarioId = _scenarioQuery.GetScenarioByAccount(requestedBy);

            if (scenarioId.HasValue == false)
            {
                return;
            }

            Scenario scenario = _scenarioRepository.Get(scenarioId.Value);
            scenario.Leave(requestedBy);
        }
    }
}
