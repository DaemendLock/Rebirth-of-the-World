using Core.Combat.Abilities;
using System;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.SpellLibrary
{
    public static class SpellLib
    {
        private static Dictionary<SpellId, Spell> _spells = new Dictionary<SpellId, Spell>();

        public static void RegisterSpell(Spell spell)
        {
            if (_spells.ContainsKey(spell.Id))
            {
                //TODO: inform spell override
            }

            _spells[spell.Id] = spell;
        }

        //TODO: Move away
        public static void LoadAllData()
        {
            throw new NotImplementedException();

            List<SpellData> data = null;

            foreach (SpellData dataItem in data)
            {
                Type type = Type.GetType(dataItem.Script) ?? typeof(Spell);

                Activator.CreateInstance(type, dataItem);
            }
        }

        public static Spell GetSpell(SpellId id)
        {
            return _spells[id];
        }

        public static void ClearAllData()
        {
            _spells.Clear();
        }
    }
}