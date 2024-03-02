using System.Collections.Generic;

using Client.Lobby.Infrastructure.Controllers;
using Client.Lobby.Infrastructure.Providers;

using Client.Lobby.Core.Characters;

using Client.Lobby.View.Gallery;
using Client.Lobby.View.Gallery.Widgets;

using Utils.Patterns.Factory;
using Client.Lobby.Core.Accounts;

namespace Client.Lobby.Infrastructure.Factories
{
    public class GalleryControllerFactory : Factory<GalleryController, Gallery>
    {
        private readonly Factory<CharacterCardWidget> _cardFactory;
        private readonly IEnumerable<Character> _characters;
        private readonly Account _account;

        public GalleryControllerFactory(Factory<CharacterCardWidget> cardFactory, IEnumerable<Character> characters, Account account)
        {
            _cardFactory = cardFactory;
            _characters = characters;
            _account = account;
        }

        public GalleryController Create(Gallery view)
        {
            CharacterGallery model = new(_characters);
            GalleryController result = new(model, view, _cardFactory, _account);

            foreach (Character character in _characters)
            {
                result.AddCharacter(character);
            }

            return result;
        }
    }

    public class CharacterCardFactory : Factory<CharacterCardWidget>
    {
        private readonly CharacterCardWidget _prefab;

        public CharacterCardFactory(AssetProvider assetProvider)
        {
            _prefab = assetProvider.CharacterCardWidgetPrefab;
        }

        public CharacterCardWidget Create()
        {
            CharacterCardWidget result = UnityEngine.Object.Instantiate(_prefab);
            return result;
        }
    }
}
