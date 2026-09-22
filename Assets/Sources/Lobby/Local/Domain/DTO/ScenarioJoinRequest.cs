using Lobby.Common.Primitives;

namespace Lobby.Local.Application.DTO
{
    public readonly struct ScenarioJoinRequest
    {
        public ScenarioJoinRequest(AccountId accountId, ScenarioId scenarioId)
        {
            ScenarioId = scenarioId;
            RequestedBy = accountId;
        }

        public ScenarioId ScenarioId { get; }
        public AccountId RequestedBy { get; }
    }
}
