using Lobby.Local.Domain.ValueObjects;

using UnityEngine;

namespace Lobby.Local.Presentation.View
{
    public sealed class LobbyTabWidget : MonoBehaviour
    {
        [field: SerializeField] public TabType Type { get; private set; }

        public bool IsActive { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    }
}
