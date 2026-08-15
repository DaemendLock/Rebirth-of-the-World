using Lobby.Common.Primitives;
using Lobby.Local.Data.Models;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Lobby.Local.Data.Repositories
{
    public sealed class AccountRepository : IAccountRepository
    {
        private readonly Dictionary<AccountId, AccountData> _values = new();

        public void Create(Account value) => _values[value.Id] = new(value);

        public void Delete(AccountId id) => _values.Remove(id);

        public Account Get(AccountId id) => _values[id].ToAccount(id);

        public void Update(Account value)
        {
            _values[value.Id].UpdateFrom(value);

            if (value.CurrentEncounter == null)
            {

            }
        }
    }
}
