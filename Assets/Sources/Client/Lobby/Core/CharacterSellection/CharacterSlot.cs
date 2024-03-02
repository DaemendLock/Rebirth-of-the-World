using Client.Lobby.Core.Characters;

using System;

namespace Client.Lobby.Core.CharacterSellection
{
    public class CharacterSlot
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
