using Core.Lobby.Characters;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Lobby.Accounts
{
    public class Account
    {
        public readonly int Id;

        public readonly Dictionary<ItemId, int> _inventory = new();
        public readonly Dictionary<int, CharacterState> _charactersData = new();

        public Account(int id)
        {
            Id = id;
        }
        // characters[]
        // string name
        // int titleId
        // background???
        // profile picture
        // byte lvl
    }
}
