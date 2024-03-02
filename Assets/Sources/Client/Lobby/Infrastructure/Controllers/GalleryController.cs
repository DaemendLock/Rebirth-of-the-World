using Client.Lobby.Core.Accounts;
using Client.Lobby.Core.Characters;

using Client.Lobby.View.Gallery;
using Client.Lobby.View.Gallery.Widgets;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Controllers
{ 
    public class GalleryController
    {
        private readonly CharacterGallery _model;
        private readonly Gallery _view;
        private readonly Factory<CharacterCardWidget> _cardFactory;
        private readonly Account _account;

        public GalleryController(CharacterGallery model, Gallery view, Factory<CharacterCardWidget> cardFactory, Account account)
        {
            _model = model;
            _view = view;
            _cardFactory = cardFactory;
            _account = account;
        }

        public void AddCharacter(Character character)
        {
            CharacterCardWidget card = _cardFactory.Create();
            _view.AddCard(card);
            card.SetCharacter(character);

            card.SetAvaible(_account.GetCharacterAvaible(character.Info.Id));
        }

        public void Update(Character character)
        {
            CharacterCardWidget card = _view.FindCharacterCard(character);
            card.SetAvaible(_account.GetCharacterAvaible(character.Info.Id));
        }
    }
}
