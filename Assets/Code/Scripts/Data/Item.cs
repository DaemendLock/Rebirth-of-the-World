using System.Collections.Generic;
using UnityEngine;

public enum ItemTags {
    CRAFTABLE,
    CRAFTED,
    EVENT,
    UNTRADABLE
}

public enum ItemQuality : byte {
    COMMON = 0,
    UNCOMMON = 1,
    RARE = 2,
    EPIC = 3,
    LEGENDARY = 4,
    EXCLUSIVE = 5,
    UNIQ = 6
}

[CreateAssetMenu(fileName = "Unnamed Item", menuName = "New Item", order = 55)]
public class Item : ScriptableObject {

    public static event System.Action<int, Item> ItemRegistred;

    public enum Category {
        BASIC,
        CONSUMABLES,
        RESORCE,
        GEAR,
        UPGRADES,
        GIFT
    }

    private static Dictionary<int, Item> _items = new Dictionary<int, Item>();

    public static Item GetById(int id) => _items[id];

    public static bool TryGetById(int id, out Item item) {
        if (_items.ContainsKey(id)) { item = _items[id]; return true; }
        item = null;
        return false;
    }

    [SerializeField] private int _id;

    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;

    [SerializeField] private Category _type;
    [SerializeField] private ItemQuality _quality;
    [SerializeField] private List<ItemTags> _tags = new List<ItemTags>();

    public Category Type => _type;

    public Sprite Icon => _icon;

    private void OnEnable() {
        _items[_id] = this;
        ItemRegistred?.Invoke(_id, this);
    }


    public Item(Category type) {
        _type = type;
    }
}