using Data.DataMapper;

namespace Data.Spells
{
    internal readonly struct SpellDataMap
    {
        public readonly MappedData View;
        public readonly MappedData CombatSpell;

        public SpellDataMap(MappedData view, MappedData combatSpell)
        {
            View = view;
            CombatSpell = combatSpell;
        }
    }
}
