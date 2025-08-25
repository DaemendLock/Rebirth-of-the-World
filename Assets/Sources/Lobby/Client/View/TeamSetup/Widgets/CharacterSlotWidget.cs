using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

using Client.Lobby.Domain.Characters;
using Client.Lobby.View.Common.CoreViews;

namespace Client.Lobby.View.TeamSetup.Widgets
{
    public class CharacterSlotWidget : BindableView<Character>, IPointerClickHandler
    {
        public bool AllowEdit = true;

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (AllowEdit)
            {
                //Lobby.Instance.OpenCharacterSelection(this);
                return;
            }

            if (Model == null)
            {
                return;
            }

            Lobby.Instance.ViewCharacter(Model);
        }

        protected override void OnModelUpdate()
        {
            _image.sprite = Model.Appearance.CardImage;
            _name.text = Model.Info.Name;
        }
    }
}
