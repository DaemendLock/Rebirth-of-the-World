using Client.Lobby.Domain.Accounts;

using Client.Lobby.Infrastructure.Networking.Requests;

using Utils.Patterns.DataProviders;

namespace Client.Lobby.Infrastructure.Providers
{
    public class AccountDataProvider : AsyncDataProvider<AccountDataType, Account>
    {
        private readonly int _accountId;
        private readonly UtilsUnity.Networking.IClient _lobbyClient;

        public AccountDataProvider(int accountId, UtilsUnity.Networking.IClient lobbyClient)
        {
            _accountId = accountId;
            _lobbyClient = lobbyClient;
        }

        protected override void OnValueRequested(AccountDataType key)
        {
            _lobbyClient.SendRequest(new AccountDataRequest(_accountId, key));
        }
    }
}
