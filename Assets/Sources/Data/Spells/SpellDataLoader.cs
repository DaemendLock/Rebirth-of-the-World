using Data.DataMapper;
using UnityEngine;
using Utils.DataTypes;

namespace Data.Spells
{
    public static class SpellDataLoader
    {
        private static readonly string _PATH = Application.streamingAssetsPath + $"{System.IO.Path.DirectorySeparatorChar}Spells{System.IO.Path.DirectorySeparatorChar}spells.datamap";
        private static readonly string _PATH_COMBAT = Application.streamingAssetsPath + $"{System.IO.Path.DirectorySeparatorChar}Spells{System.IO.Path.DirectorySeparatorChar}combatspells.data";

        private static MappedDataLoader _combat = new MappedDataLoader(_PATH_COMBAT);
        private static DataLoader<SpellId, MappedData> _spellMap = new DataLoader<SpellId, MappedData>(_PATH);

        public static bool Loaded => _spellMap.Loaded;

        public static void Load()
        {
            if (Loaded)
            {
                return;
            }

            _combat.Load();
            _spellMap.Load();
        }

        public static void Clear()
        {
            _spellMap.Clear();
            _combat.Release();
        }

        public static void Reload()
        {
            if (Loaded == false)
            {
                return;
            }

            _spellMap.Reload();
        }

        public static SpellId[] GetLoadedIds() => _spellMap.GetLoadedKeys();

        public static byte[] GetCombatSpell(SpellId spellId)
        {
            return _combat.GetBytes(_spellMap.GetData(spellId));
        }
    }
}
