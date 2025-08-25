using Client.Lobby.Domain.Common;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Utils.Patterns.DataProviders;

namespace Client.Lobby.Domain.Characters
{
    public class CharacterGallery : IUpdateableModel
    {
        public event Action Updated;

        private readonly AsyncDataProvider<int, Character> _charactersDataProvider;
        private readonly Dictionary<int, Character> _characters;

        public CharacterGallery(AsyncDataProvider<int, Character> charactersDataProvider)
        {
            _characters = new();
            _charactersDataProvider = charactersDataProvider;
        }

        public bool TryGetCharacter(int characterId, out Character character) => _characters.TryGetValue(characterId, out character);

        public async Task<Character> GetCharacter(int characterId)
        {
            if (_characters.TryGetValue(characterId, out Character character) == false)
            {
                character = await _charactersDataProvider.GetValue(characterId);
            }

            return character;
        }

        public void ViewCharacter(Character character) => _characters[character.Info.Id] = character;

        public void Update(CharacterGallery data)
        {
            bool updated = false;

            foreach (Character character in data._characters.Values)
            {
                if (_characters.TryGetValue(character.Info.Id, out Character currentValue))
                {
                    currentValue.Update(character);
                    continue;
                }

                _characters[character.Info.Id] = character;
                _charactersDataProvider.ProvideData(character.Info.Id, character);
                updated = true;
            }

            if (updated)
            {
                Updated?.Invoke();
            }
        }
    }

    public class CharacterCard
    {
        public Character character;
        public bool Avaible;
    }
}
