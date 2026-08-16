using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

using System.Linq;

namespace Lobby.Local.Domain.UseCases.Accounts
{
    public sealed class AccountGetAvailableScenariosUseCase
    {
        private readonly LobbySession _session;
        private readonly IScenarioRepository _scenarioRepository;

        public AccountGetAvailableScenariosUseCase(IScenarioRepository scenarioRepository, LobbySession session)
        {
            _scenarioRepository = scenarioRepository;
            _session = session;
        }

        public ScenarioId[] Execute()
        {
            return _scenarioRepository.GetAllIds().ToArray();
        }
    }
}
