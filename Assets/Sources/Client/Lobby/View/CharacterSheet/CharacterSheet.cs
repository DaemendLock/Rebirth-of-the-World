using UnityEngine;

using Client.Lobby.Core.Characters;

using Client.Lobby.View.Utils;
using Client.Lobby.View.Common.CoreViews;

namespace Client.Lobby.View.CharacterSheet
{
    public enum ViewMode
    {
        View,
        Edit,
        Select
    }

    public class CharacterSheet : BindableView<Character>, IMenuElement, ICharacterView
    {
        [SerializeField] private CharacterView[] _widgets;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetCharacter(Character value) => Model = value;

        protected override void OnModelUpdate()
        {
            foreach (CharacterView widget in _widgets)
            {
                widget.SetCharacter(Model);
            }
        }
    }
}