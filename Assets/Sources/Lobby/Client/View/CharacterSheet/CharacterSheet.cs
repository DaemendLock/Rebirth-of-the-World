using UnityEngine;

using Client.Lobby.Domain.Characters;

using Client.Lobby.View.Utils;
using Client.Lobby.View.Common.CoreViews;

namespace Client.Lobby.View.CharacterSheet
{
    public class CharacterSheet : BindableView<Character>, IMenuElement
    {
        [SerializeField] private CharacterView[] _widgets;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        protected override async void OnModelUpdate()
        {
            if (Model.IsLoaded == false)
            {
                await Model.Load();
            }

            foreach (CharacterView widget in _widgets)
            {
                widget.Bind(Model);
            }
        }
    }
}