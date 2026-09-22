using Game.Domain.Entities;
using Game.Domain.Repositories;

using Lobby.Common.Primitives;
using Lobby.Local.Data.Models;

using System.Collections.Generic;

namespace Lobby.Local.Data.Repositories
{
    public sealed class AccountRepository : IAccountRepository
    {
        private readonly Dictionary<AccountId, AccountData> _values = new();

        public void Create(Account account) => throw new System.NotImplementedException();
        public void Delete(AccountId id) => throw new System.NotImplementedException();
        public void Update(Account account) => throw new System.NotImplementedException();
        Account IAccountRepository.Get(AccountId id) => throw new System.NotImplementedException();
    }
}
