using Client.Lobby.Core.Accounts;
using Client.Lobby.View.Common.CoreViews;
using TMPro;
using UnityEngine;

namespace Client.Lobby.View.MainMenu.Widgets
{
    internal class ProfileWidget : BindableView<Account>
    {
        [SerializeField] private TMP_Text _lvlText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _titleText;

        protected override void OnModelUpdate()
        {
            _nameText.text = Model.Info.Name;
            _titleText.text = Model.Info.Title;
            _lvlText.text = Model.Progression.Level.Level.ToString();
        }
    }
}
