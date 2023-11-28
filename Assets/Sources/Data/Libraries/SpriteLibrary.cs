using System.Collections.Generic;
using UnityEngine;
using Utils.DataTypes;

namespace Core.Data.SpriteLib
{
    public static class SpriteLibrary
    {
        private static Dictionary<SpellId, Sprite> _loadedSprites = new Dictionary<SpellId, Sprite>();

        public static Sprite GetSpellSprite(SpellId spellId)
        {
            if (_loadedSprites.ContainsKey(spellId))
            {
                _loadedSprites.GetValueOrDefault(spellId, null);
            }

            return _loadedSprites[spellId] = SpriteLoader.LoadSpell(spellId);
        }

        public static void Clear()
        {
            _loadedSprites.Clear();
        }
    }
}
