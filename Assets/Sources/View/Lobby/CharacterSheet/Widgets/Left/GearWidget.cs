using Core.Lobby.Characters;
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
            _headSlot.ShowItem(data.Gear[0]);
            _allowEdit= allowEdit;
        }
    }
}
