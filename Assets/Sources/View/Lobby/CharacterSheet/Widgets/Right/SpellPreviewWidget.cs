using Core.Data.SpriteLib;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataTypes;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class SpellPreviewWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void ShowSpell(SpellId spell)
        {
            _icon.sprite = SpriteLibrary.GetSpellSprite(spell);
        }
    }
}
