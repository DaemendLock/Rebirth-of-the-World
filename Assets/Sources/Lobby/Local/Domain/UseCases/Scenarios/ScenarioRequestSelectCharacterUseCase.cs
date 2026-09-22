using Combat.Common.Primitives;

using Lobby.Common.Primitives;
using Lobby.Local.Application.DTO;
using Lobby.Local.Application.Outputs;
using Lobby.Local.Domain.Entities;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioRequestSelectCharacterUseCase
    {
        private readonly LobbySession _session;
        private readonly IScenarioCommandGateway _scenarioRequestGateway;

        public ScenarioRequestSelectCharacterUseCase(LobbySession session, IScenarioCommandGateway scenarioRequestGateway)
        {
            _session = session;
            _scenarioRequestGateway = scenarioRequestGateway;
        }

        public bool Execute(CharacterKey? characterId)
        {
            AccountId accountId = _session.ActiveAccountId;
            CharacterSelectInfo? selectInfo = characterId.HasValue ? new CharacterSelectInfo(characterId.Value) : null;

            ScenarioSelectCharacterRequest scenarioSelectCharacterRequest = new(accountId, selectInfo);
            return _scenarioRequestGateway.Send(scenarioSelectCharacterRequest);
        }
    }
}
