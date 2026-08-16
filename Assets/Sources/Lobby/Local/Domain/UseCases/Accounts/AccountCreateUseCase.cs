using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

using System;

namespace Lobby.Local.Domain.UseCases.Accounts
{
    public sealed class AccountFactory
    {
        private long _lastAccountId;

        public Account Create(string name, string title, int level, CharacterKey? avatarCharacterId)
        {
            AccountId accountId = new(++_lastAccountId);

            return new(accountId)
            {
                Name = name,
                Title = title,
                Level = level,
                AvatarCharacterId = avatarCharacterId
            };
        }
    }

    public sealed class AccountCreateUseCase
    {
        private readonly AccountFactory _factory;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountCreateOutput _accountCreateOutput;
        private readonly LobbySession _lobbySession;

        public AccountCreateUseCase(IAccountRepository accountRepository, IAccountCreateOutput accountCreateOutput, LobbySession lobbySession)
        {
            _accountRepository = accountRepository;
            _accountCreateOutput = accountCreateOutput;
            _lobbySession = lobbySession;
            _factory = new();
        }

        public AccountId Execute(
            string name,
            string title,
            int level,
            CharacterKey? avatarCharacterId = null)
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

            var account = _factory.Create(name, title, level, avatarCharacterId);

            _accountRepository.Create(account);
            _lobbySession.SetActiveAccount(account.Id);
            _accountCreateOutput.Present(account);
            return account.Id;
        }
    }

    public interface IAccountCreateOutput
    {
        void Present(Account account);
    }
}
