using Lobby.Common.Primitives;

namespace Lobby.Local.Application.DTO
{
    public readonly struct ScenarioLeaveRequest
    {
        public ScenarioLeaveRequest(AccountId accountId)
        {
            RequestedBy = accountId;
        }

        public AccountId RequestedBy { get; }
    }
}
