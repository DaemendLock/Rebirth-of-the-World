using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Client.Lobby.Core.Characters;
using Client.Lobby.View.Common.CoreViews;

namespace Client.Lobby.View.Gallery.Widgets
{
    public class CharacterCardWidget : BindableView<Character>, ICharacterView, IPointerClickHandler
    {
        [SerializeField] private Image _characterImage;
        [SerializeField] private TMP_Text _characterName;

        private bool _isAvaible;

        public int CharacterId => Model.Info.Id;

        public void SetCharacter(Character character) => Model = character;

        public void SetAvaible(bool avaible)
        {
            //TODO gameObject.SetActive(avaible);
        }

        public void OnPointerClick(PointerEventData eventData) => Lobby.Instance.ViewCharacter(Model);
        
        protected override void OnModelUpdate()
        {
            _characterName.text = Model.Info.Name;
            _characterImage.sprite = Model.Appearance.CardImage;
        }
    }
}
