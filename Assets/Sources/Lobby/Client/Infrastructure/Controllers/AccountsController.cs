using System.Collections.Generic;

using Client.Lobby.Domain.Accounts;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Controllers
{
    public class AccountsController
    {
        private readonly Factory<Account, int> _accountFactory;
        private readonly Dictionary<int, Account> _accountsCache;

        public AccountsController(Factory<Account, int> accountFactory)
        {
            _accountFactory = accountFactory;
            _accountsCache = new();
        }

        public Account GetAccount(int accountId)
        {
            if (_accountsCache.TryGetValue(accountId, out Account result) == false)
            {
                result = _accountFactory.Create(accountId);
                _accountsCache.Add(accountId, result);
            }

            return result;
        }
    }
}
