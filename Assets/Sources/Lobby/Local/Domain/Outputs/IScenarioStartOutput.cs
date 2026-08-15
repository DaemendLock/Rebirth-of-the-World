using Global.Local.DTO;

namespace Lobby.Local.Domain.Outputs
{
    public interface IScenarioStartOutput
    {
        void Present(StartCombatRequest request);
    }
}
