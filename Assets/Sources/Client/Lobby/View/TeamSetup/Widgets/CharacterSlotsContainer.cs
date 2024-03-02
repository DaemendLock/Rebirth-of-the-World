using Client.Lobby.Core.Characters;
using Client.Lobby.View.TeamSetup.Widgets;
using System.Collections.Generic;
using UnityEngine;

namespace View.Lobby.TeamSetup.Widgets
{
    internal class CharacterSlotsContainer : MonoBehaviour
    {
        [SerializeField] private CharacterSlotWidget _slotPrefab;

        private List<CharacterSlotWidget> _slots = new();

        public int Capacity
        {
            get
            {
                return _slots.Count;
            }

            set
            {
                Clear();

                for (int i = 0; i < value; i++)
                {
                    CreateNewSlot();
                }
            }
        }

        //public int GetRoleCount(CharacterRole role)
        //{
        //    return 0;
        //}

        private void Clear()
        {
            foreach (CharacterSlotWidget slot in _slots)
            {
                Destroy(slot.gameObject);
            }

            _slots.Clear();
        }

        private void CreateNewSlot()
        {
            CharacterSlotWidget slot = Instantiate(_slotPrefab, transform);
            _slots.Add(slot);
        }
    }
}
