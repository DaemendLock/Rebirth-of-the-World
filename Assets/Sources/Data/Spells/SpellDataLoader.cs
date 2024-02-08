using Data.Assets.Sources.Data.Spells;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils.DataTypes;

namespace Data.Spells
{
    public static class SpellDataLoader
    {
        private static readonly string _PATH_LIB = Application.streamingAssetsPath + $"{Path.DirectorySeparatorChar}Spells{Path.DirectorySeparatorChar}";

        private static ISpellDataProvider _data = new MultiFileSpellDataProvider(new List<ISpellDataProvider>() { new SpellLibFile(_PATH_LIB + "spelllib1.spelllib"), new SpellLibFile(_PATH_LIB + "spelllib2.spelllib") });

        public static void Release() => _data.Dispose();

        public static bool CanLoadSpell(SpellId id) => _data.HasSpell(id);

        public static byte[] GetCombatSpell(SpellId spellId) => _data.GetBytes(spellId);
    }
}
