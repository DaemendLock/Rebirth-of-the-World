using Lobby.Local.Presentation.ViewModels.ScenarioSelection;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Local.Presentation.View.ScenarioSelection
{
    public sealed class ScenarioView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameField;

        public void Show(ScenarioViewModel viewModel)
        {
            _icon.sprite = viewModel.Icon;
            _nameField.text = viewModel.LocalizedName;
        }
    }
}
