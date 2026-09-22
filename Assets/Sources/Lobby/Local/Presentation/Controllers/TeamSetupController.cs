using Lobby.Local.Domain.UseCases.Scenarios;

namespace Lobby.Local.Presentation.View
{
    public sealed class TeamSetupController
    {
        private readonly ScenarioRequestStartUseCase _scenarioStartUseCase;
        private readonly ScenarioRequestLeaveUseCase _scenarioLeaveUseCase;

        public TeamSetupController(ScenarioRequestStartUseCase scenarioStartUseCase, ScenarioRequestLeaveUseCase scenarioLeaveUseCase)
        {
            _scenarioStartUseCase = scenarioStartUseCase;
            _scenarioLeaveUseCase = scenarioLeaveUseCase;
        }

        public void Start()
        {
            _scenarioStartUseCase.Execute();
        }

        public void Leave()
        {
            _scenarioLeaveUseCase.Execute();
        }
    }
}
