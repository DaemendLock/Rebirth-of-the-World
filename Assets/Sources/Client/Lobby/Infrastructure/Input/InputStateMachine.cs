using System;

using UnityEngine.InputSystem;

using Client.Lobby.View.Gallery;

using Utils.DataStructure.StateMachine;
using Utils.Patterns.Factory;

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

        public void OnEnter()
        {
            _lobbyInput.MainMenu.OpenGuildFiles.performed += OpenCharacterList;
            _lobbyInput.MainMenu.Enable();
        }

        public void OnExit()
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

        private readonly Factory<LobbyInputState, Type> _stateFactory;

        public InputStateMachine()
        {

        }

        public InputStateMachine(Factory<LobbyInputState, Type> factory)
        {
            _stateFactory = factory;
        }

        public LobbyInputState CurrentState { get; private set; } = _defaultState;

        public void ChangeState(LobbyInputState state)
        {
            CurrentState.OnExit();
            CurrentState = state ?? _defaultState;
            state.OnEnter();
        }

        //  TODO: One Day
        //public void ChangeState<T>() where T : LobbyInputState
        //{
        //    LobbyInputState state = _stateFactory.Create(typeof(T));
        //    ChangeState(state);
        //}

        private class DefaultState : LobbyInputState
        {
            public void OnEnter()
            {
            }

            public void OnExit()
            {
            }
        }
    }
}
