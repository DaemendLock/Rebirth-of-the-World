using UnityEngine;
using Client.Lobby.Domain.Characters;
using Client.Lobby.View.Common.CoreViews;

using View.Lobby.CharacterSheet.Widgets;

namespace Client.Lobby.View.CharacterSheet.Widgets
{
    internal class SpellListWidget : CharacterView
    {
        [SerializeField] private SpellPreviewWidget[] _spells;

        private void Start()
        {
            foreach (SpellPreviewWidget slot in _spells)
            {
                if (slot.Spell == null)
                {
                    slot.gameObject.SetActive(false);
                }
            }
        }

        public override void Bind(Character value)
        {
            CharacterSpells spells = value.Spells;

            if (spells == null)
            {
                return;
            }

            for (int i = 0; i < spells.Count; i++)
            {
                _spells[i].SetSpell(spells.GetSpell(i));
            }
        }
    }
}
