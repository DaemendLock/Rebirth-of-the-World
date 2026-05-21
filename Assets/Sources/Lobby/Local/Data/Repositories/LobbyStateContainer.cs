using Lobby.Local.Data.Models;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

namespace Lobby.Local.Data.Repositories
{
    public sealed class LobbyStateContainer : ILobbyStateContainer
    {
        private readonly LobbyData _data = new()
        {
            ActiveTab = Domain.ValueObjects.TabType.None
        };

        public LobbyState LobbyState { get => new(_data.ActiveTab, _data.TabHistory); set => _data.Fill(value); }
    }
}
