using Core.Lobby.Accounts;
using System.Collections.Generic;
using UnityEngine;
using Utils.DataTypes;
using View.Lobby.General;
using View.Lobby.Utils;

namespace View.Lobby.Inventory
{
    internal class Inventory : MenuElement
    {
        [SerializeField] private ItemsContainer _itemsContainer;
        [SerializeField] private ItemWidget _itemPrefab;

        private void Start()
        {
            AccountData account = AccountsDataProvider._accounts[AccountsDataProvider.ActiveAccount];

            foreach (KeyValuePair<ItemId, int> item in account._inventory)
            {
                ItemWidget widget = Instantiate(_itemPrefab, _itemsContainer.transform);
                widget.ShowItem(item.Key);
            }
        }
    }
}
