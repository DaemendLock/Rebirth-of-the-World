using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Client.Lobby.Domain.Characters;
using Client.Lobby.View.Common.CoreViews;

namespace Client.Lobby.View.Gallery.Widgets
{
    public class CharacterCardWidget : BindableView<Character>, IPointerClickHandler
    {
        [SerializeField] private Image _characterImage;
        [SerializeField] private TMP_Text _characterName;
        [SerializeField] private Image _lockedIcon;

        [SerializeField] private bool _isAvaible;

        public int CharacterId => Model.Info.Id;

        public void SetAvaible(bool avaible)
        {
            _isAvaible = avaible;
            _lockedIcon.gameObject.SetActive(!avaible);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isAvaible)
            {
                Lobby.Instance.ViewCharacter(Model);
            }
        }

        protected override void OnModelUpdate()
        {
            _characterName.text = Model.Info.Name;
            _characterImage.sprite = Model.Appearance.CardImage;
        }
    }
}
