using Client.Lobby.Domain.Accounts;
using Client.Lobby.Infrastructure.Networking;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class AccountsFactoryFactory : Factory<Factory<Account, int>, UtilsUnity.Networking.IClient>
    {
        public Factory<Account, int> Create(UtilsUnity.Networking.IClient data) => new AccountFactory(data);
    }
}
