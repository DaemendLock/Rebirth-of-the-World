using Game.Domain.Entities;

using Lobby.Common.Primitives;

namespace Game.Domain.Repositories
{
    public interface IAccountRepository
    {
        void Create(Account account);
        Account Get(AccountId id);
        void Update(Account account);
        void Delete(AccountId id);
    }
}
