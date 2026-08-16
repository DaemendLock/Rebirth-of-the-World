using Lobby.Common.Primitives;

using System;

namespace Lobby.Local.Domain.Entities
{
    public sealed class LobbySession
    {
        private AccountId? _activeAccountId;
        private ScenarioId? _activeScenarioId;

        public bool HasActiveAccount => _activeAccountId.HasValue;

        public AccountId ActiveAccountId => _activeAccountId ?? throw new InvalidOperationException("The lobby does not have an active account.");

        public bool HasActiveScenario => _activeScenarioId.HasValue;

        public ScenarioId ActiveScenarioId => _activeScenarioId ?? throw new InvalidOperationException("The lobby does not have an active scenario.");

        public void SetActiveAccount(AccountId accountId)
        {
            if (_activeAccountId.HasValue && _activeAccountId.Value != accountId)
            {
                _activeScenarioId = null;
            }

            _activeAccountId = accountId;
        }

        public void ClearActiveAccount()
        {
            _activeAccountId = null;
            _activeScenarioId = null;
        }

        public void SetActiveScenario(ScenarioId scenarioId)
        {
            if (HasActiveAccount == false)
            {
                throw new InvalidOperationException("An active account is required before selecting a scenario.");
            }

            _activeScenarioId = scenarioId;
        }

        public void ClearActiveScenario()
        {
            _activeScenarioId = null;
        }
    }
}
