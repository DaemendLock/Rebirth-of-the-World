using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Lobby.View.Common
{
    internal class Button : MonoBehaviour, IPointerClickHandler
    {
        public event Action<PointerEventData> Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(eventData);
        }
    }
}