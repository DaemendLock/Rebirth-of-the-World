using UnityEngine;
using UnityEngine.EventSystems;

namespace View.Lobby.General
{
    internal class HomeButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Lobby.Instance?.Home();
        }
    }
}
