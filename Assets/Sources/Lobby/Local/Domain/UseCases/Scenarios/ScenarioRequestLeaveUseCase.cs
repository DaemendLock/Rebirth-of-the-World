using Lobby.Common.Primitives;
using Lobby.Local.Application.DTO;
using Lobby.Local.Application.Outputs;
using Lobby.Local.Domain.Entities;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioRequestLeaveUseCase
    {
        private readonly LobbySession _session;
        private readonly IScenarioCommandGateway _scenarioRequestGateway;

        public ScenarioRequestLeaveUseCase(LobbySession session, IScenarioCommandGateway scenarioRequestGateway)
        {
            _session = session;
            _scenarioRequestGateway = scenarioRequestGateway;
        }

        public void Execute()
        {
            AccountId accountId = _session.ActiveAccountId;
            _scenarioRequestGateway.Send(new ScenarioLeaveRequest(accountId));
            //ScenarioA scenario = _scenarioRepository.Get(_session.ActiveScenarioId);
            //bool scenarioChanged = false;

            //for (int i = 0; i < scenario.SelectedCharacters.Length; i++)
            //{
            //    PlayerCharacterSelection? selection = scenario.SelectedCharacters[i];

            //    if (selection.HasValue == false || selection.Value.Player != accountId)
            //    {
            //        continue;
            //    }

            //    scenario.SelectedCharacters[i] = null;
            //    scenarioChanged = true;
            //}

            //if (scenarioChanged)
            //{
            //    _scenarioRepository.Update(scenario);
            //}

            //_session.ClearActiveScenario();
        }
    }
}
