using System;
using System.Collections.Generic;

namespace View.Lobby.General.Charaters
{
    public static class CharactersList
    {
        public static Action<Character> CharacterRegistered;

        private static Dictionary<int, Character> _data = new Dictionary<int, Character>();

        public static void RegisterCharacter(int id, Character character)
        {
            _data[id] = character;
            CharacterRegistered?.Invoke(character);
        }

        public static Character GetCharacter(int id)
        {
            return _data[id];
        }
    }
}
