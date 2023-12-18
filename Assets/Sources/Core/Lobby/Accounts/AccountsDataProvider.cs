using Data.Characters;
using System;
using System.Collections.Generic;

namespace Core.Lobby.Accounts
{
    public enum AccountDataType : byte
    {
        CharacterState,
        InventoryStatus
    }

    public readonly struct AccountDataRequest
    {
        public readonly AccountDataType DataType;
        public readonly int AccountId;
        public readonly byte[] Data;

        public AccountDataRequest(AccountDataType requestType, int accountId, byte[] data)
        {
            DataType = requestType;
            AccountId = accountId;
            Data = data;
        }
    }

    public static class AccountsDataProvider
    {
        public static event Action<AccountDataRequest> AccountDataRequested;

        public static readonly Dictionary<int, AccountData> _accounts = new();

        public static int ActiveAccount { get; set; } = -1;

        public static CharacterState GetCharacterData(int characterId, int accountId)
        {
            if (_accounts.TryGetValue(accountId, out AccountData account) && account.TryGetCharacterState(characterId, out CharacterState state))
            {
                return state;
            }

            AccountDataRequested?.Invoke(new AccountDataRequest(AccountDataType.CharacterState, accountId, BitConverter.GetBytes(characterId)));

            if (_accounts.TryGetValue(accountId, out account) && account.TryGetCharacterState(characterId, out state))
            {
                return state;
            }

            throw new Exception("Failed to get charcter data.");
        }

        public static void CacheAccountData(AccountDataRequest data)
        {
            AccountData account;

            if (_accounts.ContainsKey(data.AccountId) == false)
            {
                account = new AccountData();
                _accounts[data.AccountId] = account;
            }
            else
            {
                account = _accounts[data.AccountId];
            }

            account.SaveData(data);
        }
    }
}
