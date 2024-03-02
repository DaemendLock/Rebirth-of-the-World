using UnityEngine;

namespace Client.Combat.View.Units.Spells
{
    public class SpellIconsSet
    {
        private Sprite[] _sprites = new Sprite[6];

        public Sprite GetIcon(int slot)
        {
            if (slot > _sprites.Length)
            {
                return null;
            }

            return _sprites[slot];
        }
    }
}
