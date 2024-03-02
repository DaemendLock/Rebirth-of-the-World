using Client.Lobby.Core.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client.Lobby.View.Common
{
    internal class ItemWidget : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        private Item _item;

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Tooltip.Tooltip.Show(_item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.Tooltip.Hide();
        }

        public void ShowItem(Item item)
        {
            if(item == null)
            {
                return;
            }
            _item = item;

            if (_item == null)
            {
                _icon.sprite = null;
                _icon.color = new(0, 0, 0, 0);
                return;
            }

            _icon.sprite = _item.Icon;
            _icon.color = Color.white;
        }
    }
}
