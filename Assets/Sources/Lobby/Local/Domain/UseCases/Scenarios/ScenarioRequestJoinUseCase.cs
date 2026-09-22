using Lobby.Common.Primitives;
using Lobby.Local.Application.DTO;
using Lobby.Local.Application.Outputs;
using Lobby.Local.Domain.Entities;

namespace Lobby.Local.Application.UseCases.Scenarios
{
    public sealed class ScenarioRequestJoinUseCase
    {
        private readonly LobbySession _session;
        private readonly IScenarioCommandGateway _requestGateway;

        public ScenarioRequestJoinUseCase(LobbySession session, IScenarioCommandGateway requestGateway)
        {
            _session = session;
            _requestGateway = requestGateway;
        }

        public void Execute(ScenarioId scenarioId) => _requestGateway.Send(new ScenarioJoinRequest(_session.ActiveAccountId, scenarioId));
    }
}
