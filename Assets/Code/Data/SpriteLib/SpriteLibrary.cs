using Core.Combat.Abilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Data.SpriteLib
{
    public static class SpriteLibrary
    {
        public static event Action Cleared;

        private static Dictionary<int, Sprite> _loadedSprites = new Dictionary<int, Sprite>();

        public static void LoadSpell(Spell spell)
        {
            LoadSpell(spell.Id);
        }

        public static Sprite LoadSpell(int id)
        {
            if (_loadedSprites.ContainsKey(id))
            {
                Debug.LogWarning($"Spell({id}) sprite overwrite.");
            }

            return _loadedSprites[id] = SpriteLoader.LoadSpell(id);
        }

        public static Sprite GetSpellSprite(int spellId)
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
