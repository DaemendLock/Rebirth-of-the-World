using Lobby.Common.Primitives;
using Lobby.Local.Domain.Repositories;

using System.Linq;

namespace Lobby.Local.Domain.UseCases.Accounts
{
    public sealed class AccountGetAvailableScenariosUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;

        public AccountGetAvailableScenariosUseCase(IScenarioRepository scenarioRepository)
        {
            _scenarioRepository = scenarioRepository;
        }

        public ScenarioId[] Execute(AccountId accountId)
        {
            return _scenarioRepository.GetAllIds().ToArray();
        }
    }
}
