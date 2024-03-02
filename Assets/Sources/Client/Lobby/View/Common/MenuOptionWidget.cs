using UnityEngine;
using UnityEngine.EventSystems;

using Client.Lobby.View.Utils;

namespace Client.Lobby.View.Common
{
    internal class MenuOptionWidget : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private MenuElement _target;

        public void OnPointerClick(PointerEventData eventData)
        {
            Lobby.Instance?.OpenMenu(_target);
        }
    }
}
