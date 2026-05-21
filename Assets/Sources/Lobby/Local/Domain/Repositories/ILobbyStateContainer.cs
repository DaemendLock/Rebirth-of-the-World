using Lobby.Local.Domain.Entities;

namespace Lobby.Local.Domain.Repositories
{
    public interface ILobbyStateContainer
    {
        LobbyState LobbyState { get; set; }
    }
}
