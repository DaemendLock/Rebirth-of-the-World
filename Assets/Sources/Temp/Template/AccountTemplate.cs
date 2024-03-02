using Data.Characters;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Sources.Temp.Template
{
    internal class AccountTemplate : MonoBehaviour
    {
        //TODO: refactor
        public int accountId;

        public List<CharacterDataTemplate> Characters = new();
        public List<InventoryItemTemplate> Inventory = new();

        private void OnEnable()
        {
            AccountsDataProvider.ActiveAccount = accountId;
            AccountsDataProvider.AccountDataRequested += ProvideAccountData;
        }

        private void OnDisable() => AccountsDataProvider.AccountDataRequested -= ProvideAccountData;

        private void ProvideAccountData(AccountDataRequest request)
        {
            if (request.AccountId != accountId)
            {
                return;
            }

            switch (request.DataType)
            {
                case AccountDataType.CharacterState:
                    CharacterState state = Find(BitConverter.ToInt32(request.Data)) ?? null;

                    if (state == null)
                    {
                        return;
                    }

                    byte[] data = state.GetBytes();
                    AccountsDataProvider.CacheAccountData(new AccountDataRequest(request.DataType, request.AccountId, data));
                    return;
            }
        }

        private CharacterState Find(int charId) => Characters.Find((character) => character.CharId == charId)?.GetCharacterData();
    }
}
