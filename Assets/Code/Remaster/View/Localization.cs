using System;
using System.Collections.Generic;

namespace Remaster.View
{
    public static class Localization
    {
        public static IReadOnlyDictionary<int, SpellLocaliztion> SpellLocaliztion = new Dictionary<int, SpellLocaliztion>();
        public static string Language = "";

        public static void LoadLocalization(string language)
        {
            foreach (int id in SpellLocaliztion.Keys)
            {
                throw new NotImplementedException();
            }
        }
    }

    public interface IReadOnlyLocalization
    {

    }

    public class SpellLocaliztion
    {
        public string Name;
        public string Description;
    }
}
