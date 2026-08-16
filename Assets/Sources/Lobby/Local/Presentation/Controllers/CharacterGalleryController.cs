using Lobby.Common.Primitives;
using Lobby.Local.Domain.UseCases.CharacterGallery;
using Lobby.Local.Presentation.Misc;
using Lobby.Local.Presentation.View;

namespace Lobby.Local.Presentation.Controllers
{
    public sealed class CharacterGalleryController
    {
        private readonly UiNavigationService _uiNavigationService;
        private readonly FindCharactersUseCase _findCharactersOfRoleUseCase;
        private readonly ICharacterRepository _chracterRepository;
        private readonly CharacterSheetView _characterSheetView;

        public CharacterGalleryController(FindCharactersUseCase findCharactersOfRoleUseCase, ICharacterRepository chracterRepository)
        {
            _findCharactersOfRoleUseCase = findCharactersOfRoleUseCase;
            _chracterRepository = chracterRepository;
        }

        public void OpenCharacterSheet(CharacterKey characterId)
        {
            UnityEngine.Debug.Log("Open sheet for " + characterId);

            _characterSheetView.Show(new()
            {
                LocalizedName = "Character" + characterId,
                Level = new()
            });
        }

        public CharacterKey[] LoadAll()
        {
            return _findCharactersOfRoleUseCase.Execute(default);
        }
    }
}
