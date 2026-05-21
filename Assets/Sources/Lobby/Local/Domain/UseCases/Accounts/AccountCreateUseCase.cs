using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

namespace Lobby.Local.Domain.UseCases.Accounts
{

    public sealed class AccountCreateUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountCreateOutput _accountCreateOutput;

        public AccountCreateUseCase(IAccountRepository accountRepository, IAccountCreateOutput accountCreateOutput)
        {
            _accountRepository = accountRepository;
            _accountCreateOutput = accountCreateOutput;
        }

        public AccountId Execute()
        {
            Account account = new(default);
            _accountRepository.Create(account);
            _accountCreateOutput.Present(account);
            return account.Id;
        }
    }

    public interface IAccountCreateOutput
    {
        void Present(Account account);
    }
}
