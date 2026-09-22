using Lobby.Local.Domain.Entities;
using Lobby.Local.Application.Outputs;
using Lobby.Local.Application.DTO;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    //TODO: Simplify
    public sealed class ScenarioRequestStartUseCase
    {
        private readonly IScenarioCommandGateway _requestGateway;
        private readonly LobbySession _session;

        public ScenarioRequestStartUseCase(IScenarioCommandGateway output, LobbySession session)
        {
            _requestGateway = output;
            _session = session;
        }

        public void Execute() => _requestGateway.Send(new ScenarioStartRequest(_session.ActiveScenarioId, _session.ActiveAccountId));
    }
}
