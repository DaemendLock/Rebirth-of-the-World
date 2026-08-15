using Lobby.Local.Presentation.ViewModels.ScenarioSelection;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Local.Presentation.Widgets
{
    public sealed class ScenarioCardWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameField;

        public Sprite Icon
        {
            set => _icon.sprite = value;
        }

        public string LocalizedName
        {
            set => _nameField.text = value;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
