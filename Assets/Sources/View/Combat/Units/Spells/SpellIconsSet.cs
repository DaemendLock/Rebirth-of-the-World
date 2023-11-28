using Core.Combat.Abilities;
using UnityEngine;

namespace View.Combat.Units.Spells
{
    public class SpellIconsSet
    {
        private Sprite[] _sprites = new Sprite[(int) SpellSlot.SIXTH + 1];

        public Sprite GetIcon(SpellSlot slot)
        {
            if (slot > SpellSlot.SIXTH)
            {
                return null;
            }

            return _sprites[(int) slot];
        }
    }
}
