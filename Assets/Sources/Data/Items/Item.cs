using Data.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.DataTypes;

namespace Data.Items
{
    public enum ItemType
    {
        Default,
        Material,
        Gear
    }

    [CreateAssetMenu(menuName = "Assets/Item/Item")]
    public class Item : ScriptableObject, Loadable
    {
        private static Dictionary<ItemId, Item> _items = new Dictionary<ItemId, Item>();

        [SerializeField] private Sprite _icon;
        [SerializeField] private int _id;

        public Sprite Icon => _icon;

        public Item()
        {
            Type = ItemType.Default;
        }

        public Item(ItemType type)
        {
            Type = type;
        }

        public ItemType Type { get; }

        public ItemId Id => (ItemId) _id;

        public virtual void OnLoad()
        {
            if (_items.ContainsKey((ItemId) _id))
            {
                Debug.LogWarning($"Item({_id} overwritten");
            }

            _items[(ItemId) _id] = this;
        }

        public static Item Get(ItemId id)
        {
            return _items.GetValueOrDefault(id, null);
        }

        public static Gear GetGear(ItemId id)
        {
            Item item = Get(id);

            if (item?.Type != ItemType.Gear)
            {
                return null;
            }

            return (Gear) item;
        }

        public static ItemId[] GetLoadedItemId()
        {
            return _items.Keys.ToArray();
        }
    }
}
