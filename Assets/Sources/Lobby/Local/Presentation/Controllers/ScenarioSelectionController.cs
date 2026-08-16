using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.UseCases.Accounts;
using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Presentation.ViewModels.ScenarioSelection;
using Lobby.Local.Presentation.Widgets;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View
{
    public sealed class ScenarioSelectionController : MonoBehaviour, IPointerClickHandler
    {
        [Zenject.Inject] private readonly AccountGetAvailableScenariosUseCase _getAvailableScenariosUseCase;
        [Zenject.Inject] private readonly ScenarioJoinUseCase _scenarioJoinUse;
        [Zenject.Inject] private readonly IScenarioRepository _scenarioRepository;

        private readonly Dictionary<ScenarioId, ScenarioCardWidget> _views = new();

        [SerializeField] private Transform _scenarioContainer;
        [SerializeField] private ScenarioCardWidget _prefab;
        [SerializeField] private Sprite _defaultScenarioIcon;

        [SerializeField] private TeamSetupController _teamSetupController;

        private void Start()
        {
            var values = _getAvailableScenariosUseCase.Execute();

            foreach (ScenarioId scenario in values)
            {
                AddScenario(scenario);
            }
        }

        public void Register(ScenarioId id, ScenarioCardWidget view)
        {
            if (_views.ContainsKey(id))
            {
                return;
            }
            _views.Add(id, view);
        }

        public void AddScenario(ScenarioId id)
        {
            if (_views.ContainsKey(id))
            {
                return;
            }

            Scenario data = _scenarioRepository.Get(id);

            ScenarioViewModel viewModel = new()
            {
                Icon = GetIcon(data),
                LocalizedName = data.Name,
            };

            if (_views.TryGetValue(id, out ScenarioCardWidget widget) == false)
            {
                widget = Instantiate(_prefab, _scenarioContainer);
                _views.Add(id, widget);
            }

            //widget.Show(viewModel);
        }

        private Sprite GetIcon(Scenario scenario)
        {
            return _defaultScenarioIcon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ScenarioId? scenarioId = default;

            foreach (var val in _views)
            {
                if (eventData.hovered.Contains(val.Value.gameObject) == false)
                {
                    continue;
                }

                scenarioId = val.Key;
                break;
            }

            if (scenarioId == null)
            {
                return;
            }

            _scenarioJoinUse.Execute(scenarioId.Value);
            _teamSetupController.SetupScenario(scenarioId.Value);
        }
    }
}
