using Data.DataMapper;
using Data.Spells;
using System;
using System.Collections.Generic;

namespace Data.ItemLibrary
{
    using ItemId = Int32;

    public static class ItemDataLoader
    {
        private static readonly string _PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\items.datamap";
        private static readonly string _PATH_ITEMS = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\itemspells.data";

        private static MappedDataLoader _items = new MappedDataLoader(_PATH_ITEMS);
        private static DataMap<ItemId, TwinDataMap> _itemMap;

        public static bool Loaded { get; private set; }

        public static void Load()
        {
            if (Loaded)
            {
                return;
            }

            _items.Load();
            _itemMap = new DataMap<ItemId, TwinDataMap>(_PATH);
            Loaded = true;
        }

        public static void Clear()
        {
            if (Loaded == false)
            {
                return;
            }

            _items.Release();
            _itemMap?.Release();
            Loaded = false;
        }

        public static void Reload()
        {
            Clear();
            Load();
        }

        public static ItemId[] GetLoadedIds()
        {
            return _itemMap.GetKeys();
        }

        public static byte[] GetItem(ItemId spellId)
        {
            return _items.GetBytes(_itemMap.GetData(spellId).Second);
        }

#if UNITY_EDITOR
        public static void Save(List<ItemId> ids, List<MappedData> items)
        {

        }
#endif
    }
}
