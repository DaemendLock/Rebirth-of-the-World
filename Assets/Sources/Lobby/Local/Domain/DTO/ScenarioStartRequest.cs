using Lobby.Common.Primitives;

namespace Lobby.Local.Application.DTO
{
    public readonly struct ScenarioStartRequest
    {
        public ScenarioStartRequest(ScenarioId scenarioId, AccountId requestedBy)
        {
            ScenarioId = scenarioId;
            RequestedBy = requestedBy;
        }

        public ScenarioId ScenarioId { get; }
        public AccountId RequestedBy { get; }
    }
}
