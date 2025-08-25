using Client.Lobby.Domain.Accounts;
using Client.Lobby.View.Common.CoreViews;

using TMPro;

using UnityEngine;

namespace Client.Lobby.View.MainMenu.Widgets
{
    public class ProfileWidget : BindableView<Account>
    {
        [SerializeField] private TMP_Text _lvlText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _titleText;

        protected override async void OnModelUpdate()
        {
            _nameText.text = await Model.GetName();
            _lvlText.text = (await Model.GetLevel()).Level.ToString();
        }
    }
}
