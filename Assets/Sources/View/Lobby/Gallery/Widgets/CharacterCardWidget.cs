using Data.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.Lobby.Gallery.Widgets
{
    internal class CharacterCardWidget : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _characterImage;
        [SerializeField] private TMP_Text _characterName;

        private Character _character;
        private CharacterState _data;

        public int CharacterId => _character.Id;

        public void Init(Character character, CharacterState data)
        {
            _character = character;

            _characterName.text = _character.LocalizedName;
            _characterImage.sprite = _character.GetCharacterCard(data.ViewSet);
            _data = data;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Lobby.Instance?.ViewCharacter(_character, _data);
        }
    }
}
