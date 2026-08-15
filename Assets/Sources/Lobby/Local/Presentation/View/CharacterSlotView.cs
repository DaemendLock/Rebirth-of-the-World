using Lobby.Local.Presentation.ViewModels.TeamSetup;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Local.Presentation.View
{
    public sealed class CharacterSlotView : MonoBehaviour
    {
        [SerializeField] private Image _characterIcon;
        [SerializeField] private TMP_Text _playerNameField;

        public void Show(CharacterSlowViewModel viewModel)
        {
            _characterIcon.sprite = viewModel.CharacterIcon;
            _playerNameField.text = viewModel.LocalizedName;
        }
    }
}
