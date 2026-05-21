using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Domain.UseCases.General
{
    public sealed class LobbyGoBackUseCase
    {
        private readonly ILobbyStateContainer _stateContainer;
        private readonly ILobbyOpenTabOutput _lobbyOpenTabOutput;

        public LobbyGoBackUseCase(ILobbyStateContainer stateContainer, ILobbyOpenTabOutput lobbyOpenTabOutput)
        {
            _stateContainer = stateContainer;
            _lobbyOpenTabOutput = lobbyOpenTabOutput;
        }

        public void Execute()
        {
            LobbyState state = _stateContainer.LobbyState;

            if (state.TabHistory.TryPop(out TabType tabType) == false)
            {
                return;
            }

            _lobbyOpenTabOutput.Present(tabType);
            state.ActiveTab = tabType;
            _stateContainer.LobbyState = state;
        }
    }
}
