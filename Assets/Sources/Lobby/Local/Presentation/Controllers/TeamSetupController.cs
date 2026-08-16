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
        [Zenject.Inject] private readonly IScenarioRepository _scenarioRepository;
        [Zenject.Inject] private readonly IAccountRepository _accountRepository;
        [Zenject.Inject] private readonly UiNavigationService _lobbyController;
        [Zenject.Inject] private readonly ScenarioStartUseCase _scenarioStartUseCase;

        [SerializeField] private GameObject _startButton;
        [SerializeField] private CharacterSlotView _prefab;
        [SerializeField] private Transform _slotParent;
        [SerializeField] private Sprite _defaultIcon;

        private readonly List<CharacterSlotView> _values = new();

        private ScenarioId? _id;

        public void SetupScenario(ScenarioId scenarioId)
        {
            if (_id.HasValue)
            {
                return;
            }

            _id = scenarioId;
            Scenario scenario = _scenarioRepository.Get(scenarioId);

            foreach (var val in scenario.SelectedCharacters)
            {
                AddSlot(val);
            }

            _lobbyController.OpenTab(GetComponent<LobbyTabWidget>());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.hovered.Contains(_startButton))
            {
                _scenarioStartUseCase.Execute(_id.Value);
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
