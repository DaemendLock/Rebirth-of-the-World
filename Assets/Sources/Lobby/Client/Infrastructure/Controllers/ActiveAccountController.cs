using Client.Lobby.Domain.Accounts;
using Client.Lobby.View.MainMenu.Widgets;

namespace Client.Lobby.Infrastructure.Controllers
{
    public class ActiveAccountController
    {
        private readonly ProfileWidget _profileWidget;
        private readonly GalleryController _galleryController;
        private readonly int _activeAccount;
        private Account _account;

        public ActiveAccountController(ProfileWidget profileWidget, GalleryController galleryController, int activeAccount)
        {
            _profileWidget = profileWidget;
            _galleryController = galleryController;
            _activeAccount = activeAccount;
        }

        public int ActiveAccountId => _activeAccount;

        public void SetActiveAccount(Account account)
        {
            _account = account;
            _galleryController.SetAccount(_account);
            _profileWidget.Bind(account);
        }
    }
}
