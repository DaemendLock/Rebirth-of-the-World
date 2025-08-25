using Client.Lobby.Infrastructure.Controllers;
using Client.Lobby.Infrastructure.Providers;

using Client.Lobby.Domain.Characters;

using Client.Lobby.View.Gallery;
using Client.Lobby.View.Gallery.Widgets;
using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class GalleryControllerFactory : Factory<GalleryController, Gallery>
    {
        private readonly Factory<CharacterCardWidget> _cardFactory;
        private readonly CharactersProvider _defaultCharactersProvider;

        public GalleryControllerFactory(Factory<CharacterCardWidget> cardFactory, CharactersProvider defaultCharactersProvider)
        {
            _cardFactory = cardFactory;
            _defaultCharactersProvider = defaultCharactersProvider;
        }

        public GalleryController Create(Gallery view)
        {
            foreach (Character character in _defaultCharactersProvider.GetCharacters())
            {
                CharacterCardWidget card = _cardFactory.Create();
                card.Bind(character);
                card.SetAvaible(false);
                view.AddCard(card);
            }

            return new(view);
        }
    }
}
