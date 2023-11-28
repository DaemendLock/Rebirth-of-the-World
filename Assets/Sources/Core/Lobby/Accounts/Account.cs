using Core.Lobby.Characters;
using System.Collections.Generic;

namespace Core.Lobby.Accounts
{
    public class Account
    {
        public readonly int Id;

        public readonly Dictionary<int, CharacterData> _charactersData = new();
        // characters[]
        // string name
        // int titleId
        // background???
        // profile picture
        // byte lvl
    }
}
