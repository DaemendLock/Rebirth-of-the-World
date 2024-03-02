using Client.Lobby.Core.Common;

using System;
using System.Collections.Generic;

namespace Client.Lobby.Core.Characters
{
    public class CharacterGallery : UpdateableModel
    {
        public event Action Updated;

        private readonly IEnumerable<Character> _characters;

        public CharacterGallery(IEnumerable<Character> characters)
        {
            _characters = characters;
        }

        public void SetCharacterAvaible(Character character, bool avaible)
        {
            //TODO
        }
    }

    public class CharacterCard
    {
        public Character character;
        public bool Avaible;
    }
}
