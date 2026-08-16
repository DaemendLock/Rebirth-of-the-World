using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

using System;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioJoinUseCase
    {
        private readonly LobbySession _session;
        private readonly IScenarioRepository _scenarioRepository;

        public ScenarioJoinUseCase(IScenarioRepository scenarioRepository, LobbySession session)
        {
            _scenarioRepository = scenarioRepository;
            _session = session;
        }

        public void Execute(ScenarioId scenarioId)
        {
            AccountId accountId = _session.ActiveAccountId;
            Scenario scenario = _scenarioRepository.Get(scenarioId);

            for (int i = 0; i < scenario.PlayerCount; i++)
            {
                if (scenario.SelectedCharacters[i].HasValue)
                {
                    continue;
                }

                scenario.SelectedCharacters[i] = new(accountId, default);

                return;
            }

            throw new InvalidOperationException($"Player(Id: {accountId}) can't join scenario(Id:{scenario.Id}): Scenario is full");
        }
    }
}
