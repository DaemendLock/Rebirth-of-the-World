using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Local.Presentation.View
{
    public sealed class CharacterCardWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameField;
        [SerializeField] private Image _characterArt;
        [SerializeField] private Image _lockedOverlay;

        public string Name
        {
            get => _nameField.text;
            set => _nameField.text = value;
        }

        public bool Locked
        {
            get => _lockedOverlay.enabled;
            set => _lockedOverlay.enabled = value;
        }

        public Sprite CharacterArt
        {
            set => _characterArt.sprite = value;
        }
    }
}
