using Lobby.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Lobby.Local.Domain.Entities
{
    public ref struct LobbyState
    {
        public LobbyState(TabType activeTab, Stack<TabType> tabHistory)
        {
            ActiveTab = activeTab;
            TabHistory = tabHistory;
        }

        public TabType ActiveTab { get; set; }

        public Stack<TabType> TabHistory { get; }
    }
}
