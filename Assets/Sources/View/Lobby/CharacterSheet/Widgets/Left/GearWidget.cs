using Core.Lobby.Characters;
using Data.Characters;
using UnityEngine;
using View.Lobby.General;

namespace View.Lobby.CharacterSheet.Widgets.Left
{
    internal class GearWidget : MonoBehaviour
    {
        [SerializeField] private ItemWidget _headSlot;
        [SerializeField] private ItemWidget _bodySlot;
        [SerializeField] private ItemWidget _legsSlot;
        [SerializeField] private ItemWidget _leftHandSlot;
        [SerializeField] private ItemWidget _rightHandSlot;

        private bool _allowEdit = true;

        public void ShowEquip(CharacterState data, bool allowEdit)
        {
            _allowEdit = allowEdit;

            _headSlot.ShowItem(data.GetGearInSlot((int) GearSlot.Head));
            _bodySlot.ShowItem(data.GetGearInSlot((int) GearSlot.Body));
            _legsSlot.ShowItem(data.GetGearInSlot((int) GearSlot.Legs));
            _rightHandSlot.ShowItem(data.GetGearInSlot((int) GearSlot.MainHand));
            _leftHandSlot.ShowItem(data.GetGearInSlot((int) GearSlot.OffHand));
        }
    }
}
