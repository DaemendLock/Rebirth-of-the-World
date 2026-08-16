using Lobby.Common.Primitives;
using Lobby.Local.Domain.UseCases;
using Lobby.Local.Presentation.View;
using Lobby.Local.Presentation.ViewModels;

namespace Lobby.Local.Presentation.Presenters
{
    public sealed class CharacterSheetPresenter
    {
        private readonly CharacterSheetView _view;
        private readonly GetCharacterInfoUseCase _getCharacterInfoUseCase;

        public CharacterSheetPresenter(CharacterSheetView view, GetCharacterInfoUseCase getCharacterInfoUseCase)
        {
            _view = view;
            _getCharacterInfoUseCase = getCharacterInfoUseCase;
        }

        public void Present(CharacterKey characterId, AccountId accountId)
        {
            CharacterInfo characterInfo = _getCharacterInfoUseCase.Execute(characterId, accountId);

            CharacterViewModel viewModel = new()
            {
                Level = characterInfo.Level,
                LocalizedName = characterInfo.Name
            };

            _view.Show(viewModel);
        }
    }
}
