using UnityEngine;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class SpellListWidget : MonoBehaviour
    {
        [SerializeField] private SpellPreviewWidget[] _spells;

        private void Start()
        {
            foreach (SpellPreviewWidget slot in _spells)
            {
                slot.ShowSpell(null);
            }
        }

        public void ShowSpells(Sprite[] spellIcons)
        {
            for (int i = 0; i < _spells.Length; i++)
            {
                _spells[i].ShowSpell(spellIcons.Length >= i ? spellIcons[i] : null);
            }
        }
    }
}
