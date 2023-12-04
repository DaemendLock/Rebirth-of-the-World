using Core.Lobby.Characters;
using System.Collections.Generic;

namespace Core.Lobby.Accounts
{
    public static class AccountsData
    {
        public static readonly Dictionary<int, Account> _accounts = new();

        public static int ActiveAccount { get; set; } = -1;

        public static CharacterState GetCharacterData(int characterId, int accountId)
        {
            if (_accounts.ContainsKey(accountId) == false)
            {
                return null;
            }

            Account account = _accounts[accountId];
            
            return account._charactersData[characterId];
        }
    }
}
