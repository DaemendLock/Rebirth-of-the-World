using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View
{
    public sealed class OpenTabButton : MonoBehaviour, IPointerClickHandler
    {
        [Zenject.Inject] private readonly LobbyController _lobbyController;

        [SerializeField] private LobbyTabWidget _target;

        public void OnPointerClick(PointerEventData eventData)
        {
            _lobbyController.OpenTab(_target);
        }
    }
}
