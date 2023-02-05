using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    BASIC,
    CONSUMABLES,
    RESORCE,
    GEAR,
    UPGRADES,
    GIFT
}

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

    public enum Type {
        BASIC,
        CONSUMABLES,
        RESORCE,
        GEAR,
        UPGRADES,
        GIFT
    }

    private static Dictionary<int, Item> _items = new Dictionary<int, Item>();

    public static Item GetById(int id) => _items[id];

    [SerializeField] private int _id;
    
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;

    [SerializeField] private Type _type;
    [SerializeField] private ItemQuality _quality;
    [SerializeField] private List<ItemTags> _tags = new List<ItemTags>();

    private void OnEnable() => _items[_id] = this;
    
    public Item(Type type) {
        _type = type;
    }
}