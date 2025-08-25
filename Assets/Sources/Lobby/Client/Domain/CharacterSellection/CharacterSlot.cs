using Client.Lobby.Domain.Characters;
using Client.Lobby.Domain.Common;

using System;

namespace Client.Lobby.Domain.CharacterSellection
{
    public class CharacterSlot : IUpdateableModel
    {
        public event Action Updated;

        private bool _isEditable;
        private Character _character;

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                Updated?.Invoke();
            }
        }

        public Character Character
        {
            get => _character;
            set
            {
                _character = value;
                Updated?.Invoke();
            }
        }
    }
}
