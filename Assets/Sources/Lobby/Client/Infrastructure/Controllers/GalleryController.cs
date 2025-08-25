using Client.Lobby.Domain.Accounts;

using Client.Lobby.View.Gallery;

namespace Client.Lobby.Infrastructure.Controllers
{
    public class GalleryController
    {
        private readonly Gallery _view;

        public GalleryController(Gallery view)
        {
            _view = view;
        }

        public void SetAccount(Account account)
        {
            _view.Bind(account.Characters);
        }
    }
}
