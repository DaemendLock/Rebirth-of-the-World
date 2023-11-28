using UnityEngine;
using UnityEngine.EventSystems;
using View.Lobby.Utils;

namespace View.Lobby.Gallery.Widgets
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
