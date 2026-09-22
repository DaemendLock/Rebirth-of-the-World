using Game.Application.Outputs;
using Game.Domain.Entities;

using Lobby.Common.Primitives;
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
            UnityEngine.Debug.Log($"Created scenario: Id - {scenario.Id}; Name - {scenario.Name}; Max Player Count: {scenario._members.Length}");
        void IScenarioCancelOutput.Present(ScenarioId scenarioId) =>
            UnityEngine.Debug.Log($"Scenario canceled: Id - {scenarioId}");
    }
}
