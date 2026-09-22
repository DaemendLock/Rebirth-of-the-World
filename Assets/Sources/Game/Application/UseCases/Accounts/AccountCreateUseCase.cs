using Combat.Common.Primitives;

using Game.Domain.Repositories;

using Lobby.Common.Primitives;

using System;

namespace Game.Application.UseCases
{
    public sealed class AccountCreateUseCaseA
    {
        private readonly IAccountRepository _accountRepository;

        public AccountCreateUseCaseA(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountId Execute(string name, string title, int level, CharacterKey? avatarCharacterId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("An account name is required.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("An account title is required.", nameof(title));
            }

            if (level < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Account level must be at least one.");
            }

            //var account = _factory.Create(name, title, level, avatarCharacterId);

            //_accountRepository.Create(account);
            //_lobbySession.SetActiveAccount(account.Id);
            //_accountCreateOutput.Present(account);
            return default;
        }
    }
}
