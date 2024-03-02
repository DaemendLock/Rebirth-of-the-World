using System;
using System.Collections.Generic;

using Client.Lobby.Infrastructure.Input;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    internal class InputStateFactory : Factory<LobbyInputState, Type>
    {
        private readonly Dictionary<Type, Func<LobbyInputState>> _factoryMethods;

        public void AddFactoryMethod<T>(Func<LobbyInputState> method) where T : LobbyInputState
        {
            _factoryMethods.Add(typeof(T), method);
        }

        public LobbyInputState Create(Type type) => _factoryMethods[type].Invoke();
    }
}
