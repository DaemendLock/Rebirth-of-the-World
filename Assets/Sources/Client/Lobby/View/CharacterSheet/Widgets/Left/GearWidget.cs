using UnityEngine;

using Client.Lobby.Core.Characters;

using Client.Lobby.View.Common;
using Client.Lobby.View.Common.CoreViews;

namespace Client.Lobby.View.CharacterSheet.Widgets.Left
{
    internal class GearWidget : CharacterView
    {
        [SerializeField] private ItemWidget _headSlot;
        [SerializeField] private ItemWidget _bodySlot;
        [SerializeField] private ItemWidget _legsSlot;
        [SerializeField] private ItemWidget _leftHandSlot;
        [SerializeField] private ItemWidget _rightHandSlot;

        [SerializeField] private ItemWidget _ring1Slot;
        [SerializeField] private ItemWidget _ring2Slot;

        public override void SetCharacter(Character characher)
        {
            CharacherGear gear = characher.Gear;

            return; //TODO

            _headSlot.ShowItem(gear.Head);
            _bodySlot.ShowItem(gear.Body);
            _legsSlot.ShowItem(gear.Legs);
            _rightHandSlot.ShowItem(gear.Weapon1);
            _leftHandSlot.ShowItem(gear.Weapon2);

            _ring1Slot.ShowItem(gear.Ring1);
            _ring2Slot.ShowItem(gear.Ring2);
        }
    }
}
