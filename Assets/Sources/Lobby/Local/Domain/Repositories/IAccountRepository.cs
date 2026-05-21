using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;

namespace Lobby.Local.Domain.Repositories
{
    public interface IAccountRepository
    {
        void Create(Account account);
        Account Get(AccountId id);
        void Update(Account account);
        void Delete(AccountId id);
    }
}
