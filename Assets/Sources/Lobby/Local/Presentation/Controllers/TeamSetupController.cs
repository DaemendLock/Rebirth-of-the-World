using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Domain.ValueObjects;
using Lobby.Local.Presentation.Misc;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View
{
    public sealed class TeamSetupController : MonoBehaviour, IPointerClickHandler
    {
        [Zenject.Inject] private readonly IAccountRepository _accountRepository;
        [Zenject.Inject] private readonly UiNavigationService _lobbyController;
        [Zenject.Inject] private readonly ScenarioGetActiveUseCase _getActiveScenarioUseCase;
        [Zenject.Inject] private readonly ScenarioStartUseCase _scenarioStartUseCase;
        [Zenject.Inject] private readonly ScenarioSelectCharacterUseCase _scenarioSelectCharacterUseCase;

        [SerializeField] private GameObject _startButton;
        [SerializeField] private CharacterSlotView _prefab;
        [SerializeField] private Transform _slotParent;
        [SerializeField] private Sprite _defaultIcon;

        private readonly List<CharacterSlotView> _values = new();

        public void SetupScenario()
        {
            ClearSlots();
            Scenario scenario = _getActiveScenarioUseCase.Execute();

            foreach (var val in scenario.SelectedCharacters)
            {
                AddSlot(val);
            }

            _lobbyController.OpenTab(GetComponent<LobbyTabWidget>());
        }

        private void ClearSlots()
        {
            foreach (CharacterSlotView view in _values)
            {
                Destroy(view.gameObject);
            }

            _values.Clear();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.hovered.Contains(_startButton))
            {
                _scenarioStartUseCase.Execute();
            }

            foreach (var slot in _values)
            {
                if (eventData.hovered.Contains(slot.gameObject))
                {
                    _scenarioSelectCharacterUseCase.Execute(new("katerina"));
                }
            }
        }

        private void AddSlot(PlayerCharacterSelection? value)
        {
            var view = Instantiate(_prefab, _slotParent);
            if (value.HasValue)
            {
                view.Show(new()
                {
                    LocalizedName = value.HasValue ? _accountRepository.Get(value.Value.Player).Name : "No player",
                    CharacterIcon = value.Value.CharacterKey.HasValue ? throw new System.NotImplementedException() : _defaultIcon,
                });
            }
            _values.Add(view);
        }
    }
}
