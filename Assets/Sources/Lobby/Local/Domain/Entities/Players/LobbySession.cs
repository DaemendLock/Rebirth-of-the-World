using Lobby.Common.Primitives;

using System;

namespace Lobby.Local.Domain.Entities
{
    public sealed class LobbySession
    {
        private AccountId? _activeAccountId;

        public bool HasActiveAccount => _activeAccountId.HasValue;

        public AccountId ActiveAccountId => _activeAccountId ??
            throw new InvalidOperationException("The lobby does not have an active account.");

        public void SetActiveAccount(AccountId accountId)
        {
            _activeAccountId = accountId;
        }

        public void ClearActiveAccount()
        {
            _activeAccountId = null;
        }
    }
}
