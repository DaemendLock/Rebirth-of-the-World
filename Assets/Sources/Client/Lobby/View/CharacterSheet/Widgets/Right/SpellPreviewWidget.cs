using UnityEngine;
using UnityEngine.UI;

using Client.Lobby.Core.Characters;
using Client.Lobby.View.Common.CoreViews;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class SpellPreviewWidget : MonoBehaviour, SpellView
    {
        [SerializeField] private Image _icon;

        public Spell Spell { get; private set; }

        public void SetSpell(Spell spell)
        {
            if (spell == null)
            {
                gameObject.SetActive(false);
                return;
            }

            Spell = spell;
            _icon.sprite = spell.Icon;
            gameObject.SetActive(true);
        }
    }
}
