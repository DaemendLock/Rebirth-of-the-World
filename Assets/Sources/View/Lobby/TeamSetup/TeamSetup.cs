using Core.Lobby.Encounters;
using Data.Characters;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Lobby.General;
using View.Lobby.TeamSetup.Widgets;
using View.Lobby.Utils;

namespace View.Lobby.TeamSetup
{
    public class TeamSetup : MenuElement
    {
        public static event Action<CharacterState[], Encounter> Start;

        private Encounter _encounter;

        [SerializeField] private CharacterSlotsContainer _characterSlots;
        [SerializeField] private Button _startButton;

        private void OnEnable()
        {
            _startButton.Clicked += StartCombat;
        }

        private void OnDisable()
        {
            _startButton.Clicked -= StartCombat;
        }

        public void SetUp(Encounter encounter)
        {
            _encounter = encounter;

            _characterSlots.Capacity = _encounter.PlayerCount;
        }

        private void StartCombat(PointerEventData data)
        {
            Start?.Invoke(_characterSlots.GetSelection(), _encounter);
        }
    }
}
