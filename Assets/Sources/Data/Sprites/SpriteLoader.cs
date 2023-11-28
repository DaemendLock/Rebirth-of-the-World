using UnityEngine;
using Utils.DataTypes;

namespace Core.Data.SpriteLib
{
    public static class SpriteLoader
    {
        //private DataLoader<int, SpriteDataMap>

        public static Sprite LoadSpell(SpellId spellId)
        {
            Sprite result = Resources.Load<Sprite>($"Sprites/Abilities/spell{spellId}");
            return result;
        }
    }
}
