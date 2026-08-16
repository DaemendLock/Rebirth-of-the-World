using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
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
        private readonly IAssetProvider _assetProvider;
        private readonly LobbySession _lobbySession;

        public CharacterGalleryController(FindCharactersUseCase findCharactersOfRoleUseCase, ICharacterRepository chracterRepository, CharacterSheetView characterSheetView, UiNavigationService uiNavigationService, IAssetProvider assetProvider, LobbySession lobbySession)
        {
            _findCharactersOfRoleUseCase = findCharactersOfRoleUseCase;
            _chracterRepository = chracterRepository;
            _characterSheetView = characterSheetView;
            _uiNavigationService = uiNavigationService;
            _assetProvider = assetProvider;
            _lobbySession = lobbySession;
        }

        public void OpenCharacterSheet(CharacterKey characterId)
        {
            UnityEngine.Debug.Log("Open sheet for " + characterId);

            _characterSheetView.Show(new()
            {
                CharacterKey = characterId,
                LocalizedName = characterId.ToString(),
                Level = new(),
                CardPhoto = _assetProvider.GetCharacterIcon(characterId)
            }, _lobbySession.HasActiveScenario);

            _uiNavigationService.OpenTab(_characterSheetView.GetComponent<LobbyTabWidget>());
        }

        public CharacterKey[] LoadAll() => _findCharactersOfRoleUseCase.Execute(default);
    }
}
