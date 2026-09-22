using Lobby.Local.Application.DTO;

namespace Lobby.Local.Application.Outputs
{
    public interface IScenarioCommandGateway
    {
        void Send(ScenarioStartRequest request);
        void Send(ScenarioJoinRequest request);
        bool Send(ScenarioSelectCharacterRequest request);
        void Send(ScenarioLeaveRequest request);
    }
}
