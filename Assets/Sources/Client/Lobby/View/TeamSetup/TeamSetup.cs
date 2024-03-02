using Client.Lobby.Core.Encounter;
using Client.Lobby.View.Utils;
using UnityEngine;
using View.Lobby.TeamSetup.Widgets;

namespace Client.Lobby.View.TeamSetup
{
    public class TeamSetup : MenuElement
    {
        private Encounter _encounter;

        [SerializeField] private CharacterSlotsContainer _characterSlots;

        public void SetUp(Encounter encounter)
        {
            _encounter = encounter;

            _characterSlots.Capacity = _encounter.PlayerCount;
        }

        public void AddSlot()
        {

        }
    }
}
