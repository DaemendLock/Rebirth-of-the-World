using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Local.Presentation.Widgets.CharacterSheet
{
    public sealed class CharacterPhotoWidget : MonoBehaviour
    {
        [SerializeField] private Image _modelContainer;

        public Sprite Model { get => _modelContainer.sprite; set => _modelContainer.sprite = value; }
    }
}
