using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.Lobby.General
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