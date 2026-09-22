using Game.Application.DTO;
using Game.Application.Outputs;

using Lobby.Common.Primitives;
using Lobby.Local.Application.UseCases.Scenarios;
using Lobby.Local.Presentation.ViewModels.ScenarioSelection;
using Lobby.Local.Presentation.Widgets;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View
{
    public sealed class ScenarioSelectionController : MonoBehaviour, IPointerClickHandler, IAccountAvailableScenariosOutput
    {
        [Zenject.Inject] private readonly ScenarioRequestJoinUseCase _scenarioJoinUse;

        private readonly Dictionary<ScenarioId, ScenarioCardWidget> _views = new();
        private readonly Stack<ScenarioCardWidget> _objectPool = new();

        [SerializeField] private Transform _scenarioContainer;
        [SerializeField] private ScenarioCardWidget _prefab;
        [SerializeField] private Sprite _defaultScenarioIcon;

        void IAccountAvailableScenariosOutput.Notify(AccountId accountId, ScenarioInfo[] scenarios)
        {
            foreach (var item in _views.Values)
            {
                item.Hide();
                _objectPool.Push(item);
            }

            _views.Clear();

            foreach (var scenario in scenarios)
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

        public void AddScenario(ScenarioInfo info)
        {
            if (_views.ContainsKey(info.Id))
            {
                return;
            }

            ScenarioViewModel viewModel = new()
            {
                Icon = GetIcon(info),
                LocalizedName = info.Name,
            };

            if (_views.TryGetValue(info.Id, out ScenarioCardWidget widget) == false)
            {
                if (_objectPool.TryPop(out widget) == false)
                {
                    widget = Instantiate(_prefab, _scenarioContainer);
                }

                _views.Add(info.Id, widget);
            }

            widget.Show(viewModel);
        }

        private Sprite GetIcon(ScenarioInfo scenario)
        {
            return _defaultScenarioIcon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (TryGetClickedScenario(eventData, out ScenarioId scenarioId))
            {
                _scenarioJoinUse.Execute(scenarioId);
                return;
            }
        }

        private bool TryGetClickedScenario(PointerEventData eventData, out ScenarioId result)
        {
            foreach (var val in _views)
            {
                if (eventData.hovered.Contains(val.Value.gameObject) == false)
                {
                    continue;
                }

                result = val.Key;
                return true;
            }

            result = default;
            return false;
        }
    }
}
