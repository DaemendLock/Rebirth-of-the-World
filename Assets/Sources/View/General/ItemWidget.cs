using Data.Items;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.DataTypes;

namespace View.Lobby.General
{
    internal class ItemWidget : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PointerEventData> Clicked;

        [SerializeField] private Image _icon;
        private Item _item;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_item == null)
            {
                return;
            }

            Clicked?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //TODO: Show tool tip
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //TODO: Hide tool tip
        }

        public void ShowItem(Item item)
        {
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
