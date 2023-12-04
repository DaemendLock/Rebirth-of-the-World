using Data.Items;
using System;

namespace Core.Lobby.Accounts.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        public InventoryItem(Item item, int count)
        {
            Item = item;
            Count = count;
        }

        public Item Item { get; private set; }
        public int Count { get; private set; }
    }
}
