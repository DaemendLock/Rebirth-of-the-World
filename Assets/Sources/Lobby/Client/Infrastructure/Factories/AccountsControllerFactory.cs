using Client.Lobby.Infrastructure.Controllers;

using Client.Lobby.Domain.Accounts;
using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{

    public class AccountsControllerFactory : Factory<AccountsController, Factory<Account, int>>
    {
        public AccountsController Create(Factory<Account, int> factory) => new AccountsController(factory);
    }
}
