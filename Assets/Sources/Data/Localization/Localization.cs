using System.Collections.Generic;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Data.Localization
{
    public static class Localization
    {
        private static Dictionary<string, string> _dict = new Dictionary<string, string>()
        {

        };

        public static string GetValue(string token)
        {
            return _dict.GetValueOrDefault(token, token);
        }

        public static string GetLocalizedSpellName(SpellId id) => GetValue($"Spell{id}");

        public static string GetLocalizedSpellDescription(SpellId id) => GetValue($"Spell{id}_Description");

        public static string GetStatName(UnitStat stat)
        {
            return GetValue(stat.ToString());
        }
    }
}
