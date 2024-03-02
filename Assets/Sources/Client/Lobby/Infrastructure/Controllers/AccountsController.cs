using System.Collections.Generic;

using Client.Lobby.Core.Accounts;

using Utils.DataTypes;

namespace Client.Lobby.Infrastructure.Controllers.Accounts
{
    public class AccountsController
    {
        public List<Account> Accounts = new()
        {
            new Account(new AccountInfo()
            {
                Id = 0,
                Name = "Test",
            }, new ()
            {
                Level = new(80, 0, 80, 0)
            }, new ()
            {
                new CharacterData(0, true, 0, 0, new(), new (10, 80, 1000, 2000), new ItemId[7]),
                new CharacterData(1, true,  0, 0, new(), new (20, 80, 1000, 2000), new ItemId[7])
            })
        };

        public Account GetAccount(int accountId) => Accounts.Find(value => value.Info.Id == accountId);
    }
}
