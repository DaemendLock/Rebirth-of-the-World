using System;
using System.Collections.Generic;

namespace Remaster.SpellLib
{
    public static class SpellLib
    {
        private static Dictionary<int, Spell> _spells = new Dictionary<int, Spell> ();

        public static void RegisterSpell(Spell spell)
        {
            if (_spells.ContainsKey(spell.Id))
            {
                //TODO: inform spell override
            }

            _spells[spell.Id] = spell;
        }

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

        public static Spell GetSpell(int id)
        {
            return _spells[id]; 
        }

        public static void ClearAllData()
        {
            _spells.Clear();
        }
    }
}