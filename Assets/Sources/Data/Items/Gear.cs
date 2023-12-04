using UnityEngine;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Data.Items
{
    public enum GearType
    {
        Head,
        Body,
        Legs,
        Weapon,
        Ring,
        Consumable
    }

    public abstract class Gear : Item
    {
        [SerializeField] private bool _hasSpell;
        [SerializeField] private int _spellId;
        [SerializeField] private StatsTable _stats;

        public Gear(GearType type) : base(ItemType.Gear)
        {
            GearType = type;
        }

        public GearType GearType { get; }

        public StatsTable Stats => _stats;

        public bool HasSpell => _hasSpell;

        public SpellId SpellId => (SpellId) _spellId; 
    }
}
