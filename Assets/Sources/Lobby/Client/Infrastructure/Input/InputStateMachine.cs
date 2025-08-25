using UnityEngine.InputSystem;

using Client.Lobby.View.Gallery;
using Utils.Patterns.StateMachine;

namespace Client.Lobby.Infrastructure.Input
{
    public interface LobbyInputState : IState
    {
    }

    public class MainMenuInputState : LobbyInputState
    {
        private readonly LobbyInput _lobbyInput;
        private readonly Gallery _gallery;

        public MainMenuInputState(LobbyInput lobbyInput, Gallery gallery)
        {
            _lobbyInput = lobbyInput;
            _gallery = gallery;
        }

        public void Enter()
        {
            _lobbyInput.MainMenu.OpenGuildFiles.performed += OpenCharacterList;
            _lobbyInput.MainMenu.Enable();
        }

        public void Exit()
        {
            _lobbyInput.MainMenu.Disable();
            _lobbyInput.MainMenu.OpenGuildFiles.performed -= OpenCharacterList;
        }

        private void OpenCharacterList(InputAction.CallbackContext callback)
        {
            View.Lobby.Instance?.OpenMenu(_gallery);
        }
    }

    public class InputStateMachine : IStateMachine<LobbyInputState>
    {
        private static readonly LobbyInputState _defaultState = new DefaultState();

        public LobbyInputState CurrentState { get; private set; } = _defaultState;

        public void ChangeState(LobbyInputState state)
        {
            CurrentState.Exit();
            CurrentState = state ?? _defaultState;
            state.Enter();
        }

        private class DefaultState : LobbyInputState
        {
            public void Enter()
            {
            }

            public void Exit()
            {
            }
        }
    }
}
