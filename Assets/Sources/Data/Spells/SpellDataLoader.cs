using Data.DataMapper;
using System;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Data.Spells
{
    public static class SpellDataLoader
    {
        private static readonly string _PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\spells.datamap";
        private static readonly string _PATH_COMBAT = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\combatspells.data";
        private static readonly string _PATH_VIEW = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\viewspells.data";

        private static MappedDataLoader _combat = new MappedDataLoader(_PATH_COMBAT);
        private static MappedDataLoader _view = new MappedDataLoader(_PATH_VIEW);
        private static DataMap<SpellId, SpellDataMap> _spellMap;

        public static bool Loaded { get; private set; }

        public static void Load()
        {
            if (Loaded)
            {
                return;
            }

            _combat.Load();
            _view.Load();
            _spellMap = new DataMap<SpellId, SpellDataMap>(_PATH);
            Loaded = true;
        }

        public static void Clear()
        {
            if(Loaded == false)
            {
                return;
            }

            _combat.Release();
            _view.Release();
            _spellMap?.Release();
            Loaded = false;
        }

        public static void Reload()
        {
            Clear();
            Load();
        }

        public static SpellId[] GetLoadedIds()
        {
            return _spellMap.GetKeys();
        }

        public static byte[] GetSpellView(SpellId spellId)
        {
            return _view.GetBytes(_spellMap.GetData(spellId).View);
        }

        public static byte[] GetCombatSpell(SpellId spellId)
        {
            return _combat.GetBytes(_spellMap.GetData(spellId).CombatSpell);
        }

#if UNITY_EDITOR
        public static void Save(List<SpellId> ids, List<MappedData> view, List<MappedData> combat)
        {

        }
#endif
    }
}
