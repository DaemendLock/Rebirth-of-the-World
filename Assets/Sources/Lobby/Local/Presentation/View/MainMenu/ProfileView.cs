using Lobby.Local.Presentation.ViewModels.MainMenu;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Local.Presentation.View.MainMenu
{
    public sealed class ProfileView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameField;
        [SerializeField] private TMP_Text _titleField;
        [SerializeField] private TMP_Text _levelField;
        [SerializeField] private Image _image;

        public Sprite CurrentAvatar => _image.sprite;

        public void Show(ProfileViewModel viewModel)
        {
            _nameField.text = viewModel.Name;
            _titleField.text = viewModel.LocalizedTitle;
            _levelField.text = $"Lv.{viewModel.Level}";
            _image.sprite = viewModel.Avatar;
        }
    }
}
