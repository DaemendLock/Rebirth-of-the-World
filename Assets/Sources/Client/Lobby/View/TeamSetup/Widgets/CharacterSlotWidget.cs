using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

using Client.Lobby.Core.Characters;
using Client.Lobby.View.Common.CoreViews;

namespace Client.Lobby.View.TeamSetup.Widgets
{
    public class CharacterSlotWidget : BindableView<Character>, ICharacterView, IPointerClickHandler
    {
        public bool AllowEdit = true;

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;

        private Character _character;

        public void SetCharacter(Character character)
        {
            Model = character;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (AllowEdit)
            {
                Lobby.Instance.OpenCharacterSelection(this);
                return;
            }

            if (_character == null)
            {
                return;
            }

            Lobby.Instance.ViewCharacter(_character);
        }

        protected override void OnModelUpdate()
        {
            _image.sprite = Model.Appearance.CardImage;
            _name.text = Model.Info.Name;
        }
    }
}
