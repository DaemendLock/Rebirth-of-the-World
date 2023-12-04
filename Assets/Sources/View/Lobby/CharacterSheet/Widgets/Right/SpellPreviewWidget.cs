using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class SpellPreviewWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void ShowSpell(Sprite icon)
        {
            if(icon == null)
            {
                gameObject.SetActive(false);
                return;
            }

            _icon.sprite = icon;
            gameObject.SetActive(true);
        }
    }
}
