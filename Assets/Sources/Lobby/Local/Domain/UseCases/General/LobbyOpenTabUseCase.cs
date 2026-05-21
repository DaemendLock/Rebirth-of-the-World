using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Domain.UseCases.General
{
    public sealed class LobbyOpenTabUseCase
    {
        private readonly ILobbyStateContainer _stateContainer;
        private readonly ILobbyOpenTabOutput _lobbyOpenTabOutput;

        public LobbyOpenTabUseCase(ILobbyStateContainer stateContainer, ILobbyOpenTabOutput lobbyOpenTabOutput)
        {
            _stateContainer = stateContainer;
            _lobbyOpenTabOutput = lobbyOpenTabOutput;
        }

        public void Execute(TabType tabType)
        {
            LobbyState state = _stateContainer.LobbyState;

            if (state.ActiveTab == tabType)
            {
                return;
            }

            _lobbyOpenTabOutput.Present(tabType);
            state.TabHistory.Push(state.ActiveTab);
            state.ActiveTab = tabType;
            _stateContainer.LobbyState = state;
        }
    }

    public interface ILobbyOpenTabOutput
    {
        void Present(TabType tabType);
    }
}
