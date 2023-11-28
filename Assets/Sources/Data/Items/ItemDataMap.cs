using Data.DataMapper;

namespace Data.Items
{
    internal readonly struct ItemDataMap
    {
        public readonly MappedData View;
        public readonly MappedData CombatItem;

        public ItemDataMap(MappedData view, MappedData combatItem)
        {
            View = view;
            CombatItem = combatItem;
        }
    }
}
