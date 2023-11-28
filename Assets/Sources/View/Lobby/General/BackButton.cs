using UnityEngine;
using UnityEngine.EventSystems;

namespace View.Lobby.General
{
    internal class BackButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Lobby.Instance?.GoBack();
        }
    }
}
