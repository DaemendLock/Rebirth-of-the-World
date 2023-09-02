using UnityEngine;
using Utils.DataTypes;

namespace Core.Data.SpriteLib
{
    public static class SpriteLoader
    {
        public static Sprite LoadSpell(SpellId spellId)
        {
            Sprite result = Resources.Load<Sprite>($"Sprites/Abilities/spell{spellId}");
            Debug.Log(result);
            return result;
        }
    }
}
