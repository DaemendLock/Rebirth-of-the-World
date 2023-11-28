using Data.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View.Lobby.General.Charaters;

namespace View.Lobby.Gallery.Widgets
{
    internal class CharacterCardWidget : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _characterImage;
        [SerializeField] private TMP_Text _characterName;

        private Character _character;

        public void Init(Character character)
        {
            _character = character;

            //_characterImage.sprite = _character.Picture;
            _characterName.text = Localization.GetValue(_character.NameToken);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Lobby.Instance?.ViewCharacter(_character);
        }
    }
}
