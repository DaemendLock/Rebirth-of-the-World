using Core.Lobby.Characters;
using System.Collections.Generic;

namespace Core.Lobby.Accounts
{
    public class AccountsData
    {
        private readonly Dictionary<int, Account> _accounts;

        public CharacterData GetCharacterData(int characterId, int accountId)
        {
            if (_accounts.ContainsKey(accountId) == false)
            {
                return null;
            }

            Account account = _accounts[accountId];

            return null;
        }
    }
}
