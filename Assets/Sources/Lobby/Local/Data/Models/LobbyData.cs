using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Lobby.Local.Data.Models
{
    public sealed class LobbyData
    {
        public TabType ActiveTab { get; set; }
        public Stack<TabType> TabHistory { get; } = new();

        public void Fill(LobbyState state)
        {
            ActiveTab = state.ActiveTab;
        }
    }
}
