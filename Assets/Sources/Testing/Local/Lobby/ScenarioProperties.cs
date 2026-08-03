using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Presentation.View;
using Lobby.Local.Presentation.Widgets;

using System;

using UnityEngine;

namespace Testing.Local.Lobby
{
    [RequireComponent(typeof(ScenarioCardWidget))]
    public sealed class ScenarioProperties : MonoBehaviour
    {
        [Zenject.Inject] private ScenarioCreateUseCase _scenarioCreateUseCase;
        [Zenject.Inject] private ScenarioCancelUseCase _scenarioCancelUseCase;

        [SerializeField] private string _scenarioName;
        [SerializeField] private int _maxPlayerCount;

        [SerializeField] private string _guid;

        private void Start()
        {
            var val = _scenarioCreateUseCase.Execute(_scenarioName, _maxPlayerCount);
            _guid = val.ToString();
            FindAnyObjectByType<ScenarioSelectionController>().Register(val, GetComponent<ScenarioCardWidget>());
        }

        private void OnDestroy()
        {
            if (string.IsNullOrEmpty(_guid))
            {
                return;
            }

            _scenarioCancelUseCase.Execute(new(Guid.Parse(_guid)));
        }
    }
}