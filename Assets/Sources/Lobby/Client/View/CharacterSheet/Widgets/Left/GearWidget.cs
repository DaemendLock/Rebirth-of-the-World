using UnityEngine;

using Client.Lobby.Domain.Characters;

using Client.Lobby.View.Common;
using Client.Lobby.View.Common.CoreViews;
using System.Collections.Generic;

namespace Client.Lobby.View.CharacterSheet.Widgets.Left
{
    internal class GearWidget : CharacterView
    {
        [SerializeField] private List<ItemWidget> _itemWidgets;

        public override void Bind(Character characher)
        {
            IEquipmentInfo gear = characher.Gear;

            foreach (ItemWidget widget in _itemWidgets)
            {
                widget.ShowItem(gear.GetItem(widget.Slot));
            }
        }
    }
}
