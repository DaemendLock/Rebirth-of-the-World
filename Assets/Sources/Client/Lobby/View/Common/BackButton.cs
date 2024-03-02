using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Lobby.View.Common
{
    internal class BackButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Lobby.Instance?.GoBack();
        }
    }
}
