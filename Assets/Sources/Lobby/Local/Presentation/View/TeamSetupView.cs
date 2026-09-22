using Game.Application.Outputs;
using Game.Domain.ValueObjects;

using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Presentation.Misc;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View
{
    public sealed class TeamSetupView : MonoBehaviour, IPointerClickHandler, IAccountScenarioJoinOutput
    {
        [Zenject.Inject] private readonly UiNavigationService _lobbyController;
        [Zenject.Inject] private readonly TeamSetupController _teamSetupController;
        [Zenject.Inject] private readonly LobbySession _lobbySession;

        [SerializeField] private GameObject _startButton;
        [SerializeField] private GameObject _leaveButton;
        [SerializeField] private CharacterSlotView _prefab;
        [SerializeField] private Transform _slotParent;
        [SerializeField] private Sprite _defaultIcon;
        [SerializeField] private LobbyTabWidget _galleryTab;

        private readonly List<CharacterSlotView> _values = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.hovered.Contains(_leaveButton))
            {
                _teamSetupController.Leave();
                ClearSlots();
                _lobbyController.GoHome();
                return;
            }

            if (eventData.hovered.Contains(_startButton))
            {
                _teamSetupController.Start();
                return;
            }

            foreach (var slot in _values)
            {
                if (eventData.hovered.Contains(slot.gameObject))
                {
                    _lobbyController.OpenTab(_galleryTab);
                    return;
                }
            }
        }

        void IAccountScenarioJoinOutput.Present(AccountId accountId, ScenarioId scenarioId, ScenarioMemberInfo?[] members)
        {
            ClearSlots();

            foreach (var val in members)
            {
                AddSlot(val);
            }

            _lobbyController.OpenTab(GetComponent<LobbyTabWidget>());
            _lobbySession.SetActiveScenario(scenarioId);
        }

        private void ClearSlots()
        {
            foreach (CharacterSlotView view in _values)
            {
                Destroy(view.gameObject);
            }

            _values.Clear();
        }

        private void AddSlot(ScenarioMemberInfo? value)
        {
            var view = Instantiate(_prefab, _slotParent);
            if (value.HasValue)
            {
                view.Show(new()
                {
                    //LocalizedName = value.HasValue ? _accountRepository.Get(value.Value.AccountId).Name : "No player",
                    CharacterIcon = value.Value.Character.HasValue ? throw new System.NotImplementedException() : _defaultIcon,
                });
            }
            _values.Add(view);
        }
    }
}
