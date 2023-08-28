using UnityEngine;

namespace Remaster.Data.SpriteLib
{
    public static class SpriteLoader
    {
        public static Sprite LoadSpell(int spellId)
        {
            Sprite result = Resources.Load<Sprite>($"Sprites/Abilities/spell{spellId}");
            Debug.Log(result);
            return result;
        }
    }
}
