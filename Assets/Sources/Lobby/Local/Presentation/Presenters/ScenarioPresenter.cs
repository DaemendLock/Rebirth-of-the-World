using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.UseCases.Scenarios;

namespace Lobby.Local.Presentation.Presenters
{
    public sealed class ScenarioPresenter : IScenarioCreateOutput, IScenarioCancelOutput
    {
        void IScenarioCreateOutput.Present(Scenario scenario) =>
            UnityEngine.Debug.Log($"Created scenario: Id - {scenario.Id}; Name - {scenario.Name}; Max Player Count: {scenario.PlayerCount}");
        void IScenarioCancelOutput.Present(ScenarioId scenarioId) =>
            UnityEngine.Debug.Log($"Scenario canceled: Id - {scenarioId}");
    }
}
