using Game.Application.DTO;
using Game.Application.Repositories;
using Game.Domain.Entities;
using Game.Domain.Repositories;

using Lobby.Common.Primitives;

namespace Game.Application.UseCases.Scenarios
{
    public sealed class ScenarioSelectCharacterUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IScenarioQueryGateway _scenarioQuery;

        public ScenarioSelectCharacterUseCase(IScenarioRepository scenarioRepository, IScenarioQueryGateway scenarioQuery)
        {
            _scenarioRepository = scenarioRepository;
            _scenarioQuery = scenarioQuery;
        }

        public bool Execute(SelectCharacterInfo command)
        {
            AccountId accountId = command.RequestedBy;
            ScenarioId? target = _scenarioQuery.GetScenarioByAccount(accountId);

            if (target.HasValue == false)
            {
                return false;
            }

            Scenario scenario = _scenarioRepository.Get(target.Value);

            try
            {
                scenario.SelecetCharacter(accountId, command.CharacterSelection);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
