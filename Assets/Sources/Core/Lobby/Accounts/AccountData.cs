using Data.Characters;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Lobby.Accounts
{
    public class AccountData
    {
        public readonly Dictionary<ItemId, int> _inventory = new();
        public readonly Dictionary<int, CharacterState> _charactersData = new();

        // characters[]
        // string name
        // int titleId
        // background???
        // profile picture
        // byte lvl

        public bool TryGetCharacterState(int id, out CharacterState result)
        {
            if (_charactersData.ContainsKey(id) == false)
            {
                result = default;
                return false;
            }

            result = _charactersData[id];
            return true;
        }

        public void SaveData(AccountDataRequest data)
        {
            switch (data.DataType)
            {
                case AccountDataType.CharacterState:
                    SaveCharacterState(data.Data);
                    return;

                default:
                    break;
            }
        }

        private void SaveCharacterState(byte[] data)
        {
            CharacterState state = CharacterState.Parse(data);
            _charactersData[state.CharacterId] = state;

        }
    }
}
