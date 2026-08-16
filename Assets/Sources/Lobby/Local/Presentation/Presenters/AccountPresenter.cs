using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.UseCases.Accounts;

using Lobby.Local.Domain.Repositories;
using Lobby.Local.Presentation.Misc;
using Lobby.Local.Presentation.View.MainMenu;
using Lobby.Local.Presentation.ViewModels.MainMenu;

using UnityEngine;

namespace Lobby.Local.Presentation.Presenters
{
    public sealed class AccountPresenter : IAccountCreateOutput
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ProfileView _view;

        public AccountPresenter(IAssetProvider assetProvider, ProfileView view)
        {
            _assetProvider = assetProvider;
            _view = view;
        }

        public void PresentCurrent(Account account)
        {
            Sprite avatar = _view.CurrentAvatar;

            if (account.AvatarCharacterId.HasValue)
            {
                Sprite accountAvatar = _assetProvider.GetCharacterIcon(account.AvatarCharacterId.Value);
                avatar = accountAvatar != null ? accountAvatar : avatar;
            }

            _view.Show(new ProfileViewModel
            {
                Name = account.Name,
                LocalizedTitle = account.Title,
                Level = account.Level,
                Avatar = avatar
            });
        }

        void IAccountCreateOutput.Present(Account account)
        {
            PresentCurrent(account);
        }
    }
}
