using Core.Combat.Gear;
using Data.ItemLibrary;
using System.Collections.Generic;
using Utils.Serializer;

namespace Core.ItemLibrary
{
    using ItemId = System.Int32;

    public static class ItemLib
    {
        private static Dictionary<ItemId, Gear> _items = new Dictionary<ItemId, Gear>();

        public static void LoadData(ItemId[] ids)
        {
            ItemDataLoader.Load();

            foreach (ItemId id in ids)
            {
                RegisterItem(ItemSerializer.Deserialize(ItemDataLoader.GetItem(id)));
            }
        }

        //TODO: Move away
        public static void LoadAllData()
        {
            ItemDataLoader.Load();

            ItemId[] ids = ItemDataLoader.GetLoadedIds();

            foreach (ItemId id in ids)
            {
                RegisterItem(ItemSerializer.Deserialize(ItemDataLoader.GetItem(id)));
            }
        }

        public static Gear GetItem(ItemId id)
        {
            if (_items.ContainsKey(id) == false)
            {
                return null;
            }

            return _items[id];
        }

        public static void ClearAllData()
        {
            _items.Clear();
        }

        internal static void RegisterItem(Gear item)
        {
            if (_items.ContainsKey(item.Id))
            {
                return;
            }

            _items[item.Id] = item;
        }
    }
}
