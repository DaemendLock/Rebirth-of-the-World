using Core.Lobby.Accounts;
using System.Collections.Generic;
using UnityEngine;
using Utils.DataTypes;

namespace Assets.Sources.Temp.Template
{
    internal class AccountTemplate : MonoBehaviour
    {
        public int accountId;

        public List<CharacterDataTemplate> Characters = new();
        public List<InventoryItemTemplate> Inventory = new();

        private void Start()
        {
            if (AccountsData.ActiveAccount == -1)
            {
                AccountsData.ActiveAccount = accountId;
            }

            Account account = new Account(accountId);

            AccountsData._accounts[accountId] = account;

            foreach (CharacterDataTemplate template in Characters)
            {
                account._charactersData[template.charId] = template.GetCharacterData();
            }

            foreach (InventoryItemTemplate template in Inventory)
            {
                account._inventory[(ItemId) template.ItemId] = template.Count;
            }
        }
    }
}
