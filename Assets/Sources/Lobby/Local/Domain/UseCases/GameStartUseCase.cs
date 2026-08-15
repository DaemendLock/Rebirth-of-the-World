using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

using UnityEngine.SceneManagement;

namespace Lobby.Local.Domain.UseCases
{
    public sealed class GameStartUseCase
    {
        private readonly IAccountRepository _accountRepository;

        public void Execute(AccountId accountId)
        {
            Account account = _accountRepository.Get(accountId);

            if (account.CurrentEncounter.HasValue)
            {
                SendToLobby();
                return;
            }

            SendToEncounter(account.CurrentEncounter.Value);
        }

        private void SendToLobby()
        {
            SceneManager.LoadScene("Lobby");
        }

        private void SendToEncounter(EncounterId encounterId)
        {
            SceneManager.LoadScene("test");
        }
    }
}
