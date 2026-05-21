using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Domain.UseCases.General
{
    public sealed class LobbyGoHomeUseCase
    {
        private readonly ILobbyStateContainer _stateContainer;
        private readonly ILobbyOpenTabOutput _lobbyOpenTabOutput;

        public LobbyGoHomeUseCase(ILobbyStateContainer stateContainer, ILobbyOpenTabOutput lobbyOpenTabOutput)
        {
            _stateContainer = stateContainer;
            _lobbyOpenTabOutput = lobbyOpenTabOutput;
        }

        public void Execute()
        {
            LobbyState state = _stateContainer.LobbyState;

            state.TabHistory.Clear();

            _lobbyOpenTabOutput.Present(TabType.MainMenu);
            state.ActiveTab = TabType.MainMenu;
            _stateContainer.LobbyState = state;
        }
    }
}
