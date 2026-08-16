using Lobby.Common.Primitives;
using Lobby.Local.Presentation.View;

using System;

namespace Lobby.Local.Presentation.Presenters
{
    public sealed class CharacterGalleryPresenter
    {
        [Zenject.Inject] private CharacterGalleryView _view;

        public void Present(Span<CharacterKey> characters)
        {
            foreach (var character in characters)
            {
                ViewModels.CharacterCardViewModel viewModel = new(character);
                _view.Show(viewModel);
            }
        }
    }
}
