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
        private static DataLoader<SpellId, TwinDataMap> _spellMap = new DataLoader<SpellId, TwinDataMap>(_PATH);

        public static bool Loaded => _spellMap.Loaded;

        public static void Load()
        {
            _combat.Load();
            _view.Load();
            _spellMap.Load();
        }

        public static void Clear() => _spellMap.Clear();

        public static void Reload() => _spellMap.Reload();

        public static SpellId[] GetLoadedIds() => _spellMap.GetLoadedKeys();

        public static byte[] GetSpellView(SpellId spellId)
        {
            return _view.GetBytes(_spellMap.GetData(spellId).First);
        }

        public static byte[] GetCombatSpell(SpellId spellId)
        {
            return _combat.GetBytes(_spellMap.GetData(spellId).Second);
        }

#if UNITY_EDITOR
        public static void Save(List<SpellId> ids, List<MappedData> view, List<MappedData> combat)
        {

        }
#endif
    }
}
