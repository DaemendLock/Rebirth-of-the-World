using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Lobby.View.Common
{
    internal class HomeButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Lobby.Instance?.Home();
        }
    }
}
