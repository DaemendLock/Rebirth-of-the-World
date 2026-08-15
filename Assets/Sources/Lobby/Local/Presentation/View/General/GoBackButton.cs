using Lobby.Local.Presentation.Misc;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View
{
    public sealed class GoBackButton : MonoBehaviour, IPointerClickHandler
    {
        [Zenject.Inject] private readonly UiNavigationService _lobbyController;

        public void OnPointerClick(PointerEventData eventData)
        {
            _lobbyController.GoBack();
        }
    }
}
