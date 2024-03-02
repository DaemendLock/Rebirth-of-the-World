using Client.Lobby.View.Common;
using Client.Lobby.View.Utils;
using UnityEngine;

namespace Client.Lobby.View.Inventory
{
    public class Inventory : MenuElement
    {
        [SerializeField] private ItemsContainer _itemsContainer;
        [SerializeField] private ItemWidget _itemPrefab;

        private void Start()
        {
            //AccountData account = AccountsDataProvider._accounts[AccountsDataProvider.ActiveAccount];

            //foreach (KeyValuePair<ItemId, int> item in account._inventory)
            //{
            //    ItemWidget widget = Instantiate(_itemPrefab, _itemsContainer.transform);
            //    widget.ShowItem(Item.Get(item.Key));
            //}
        }
    }
}
