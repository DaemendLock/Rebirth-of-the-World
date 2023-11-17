using Core.Combat.Abilities;
using Data.Spells;
using System.Collections.Generic;
using Utils.DataTypes;
using Utils.Serializer;

namespace Core.SpellLibrary
{
    public static class SpellLib
    {
        private static Dictionary<SpellId, Spell> _spells = new Dictionary<SpellId, Spell>();

        //TODO: Move away
        public static void LoadAllData()
        {
            SpellDataLoader.Load();

            SpellId[] ids = SpellDataLoader.GetLoadedIds();

            foreach (SpellId id in ids)
            {
                SpellSerializer.FromSpellData(SpellSerializer.Deserialize(SpellDataLoader.GetCombatSpell(id)));
            }
        }

        public static Spell GetSpell(SpellId id)
        {
            if (_spells.ContainsKey(id) == false)
            {
                return null;
            }

            return _spells[id];
        }

        public static void ClearAllData()
        {
            _spells.Clear();
        }

        internal static void RegisterSpell(Spell spell)
        {
            if (_spells.ContainsKey(spell.Id))
            {
                return;
            }

            _spells[spell.Id] = spell;
        }
    }
}