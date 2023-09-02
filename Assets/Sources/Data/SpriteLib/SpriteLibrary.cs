using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.DataTypes;

namespace Core.Data.SpriteLib
{
    public static class SpriteLibrary
    {
        public static event Action Cleared;

        private static Dictionary<SpellId, Sprite> _loadedSprites = new Dictionary<SpellId, Sprite>();

        public static Sprite LoadSpell(SpellId id)
        {
            if (_loadedSprites.ContainsKey(id))
            {
                Debug.LogWarning($"Spell({id}) sprite overwrite.");
            }

            return _loadedSprites[id] = SpriteLoader.LoadSpell(id);
        }

        public static Sprite GetSpellSprite(SpellId spellId)
        {
            return _loadedSprites.GetValueOrDefault(spellId, null);
        }

        public static void Clear()
        {
            _loadedSprites.Clear();
            Cleared?.Invoke();
        }
    }
}
