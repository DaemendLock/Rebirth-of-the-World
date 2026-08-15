using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Presentation.View;

namespace Lobby.Local.Presentation.Presenters
{
    public enum ScenarioTab
    {
        MainStory,
        MaterialGather,
        Events,
    }

    public sealed class ScenarioWindowPresenter : IScenarioCreateOutput, IScenarioCancelOutput
    {
        private readonly ScenarioWindowView _scenarioSelectionView;

        void IScenarioCreateOutput.Present(Scenario scenario) =>
            UnityEngine.Debug.Log($"Created scenario: Id - {scenario.Id}; Name - {scenario.Name}; Max Player Count: {scenario.PlayerCount}");
        void IScenarioCancelOutput.Present(ScenarioId scenarioId) =>
            UnityEngine.Debug.Log($"Scenario canceled: Id - {scenarioId}");
    }
}
