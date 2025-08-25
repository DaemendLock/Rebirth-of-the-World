using Client.Lobby.Domain.Accounts;
using Client.Lobby.Infrastructure.Providers;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class AccountFactory : Factory<Account, int>
    {
        private readonly UtilsUnity.Networking.IClient _lobbyClient;

        public AccountFactory(UtilsUnity.Networking.IClient supply)
        {
            _lobbyClient = supply;
        }

        public Account Create(int id) => new(id, new AccountDataProvider(id, _lobbyClient),
            new CharacterPartialDataProvider(_lobbyClient, id));
    }
}
