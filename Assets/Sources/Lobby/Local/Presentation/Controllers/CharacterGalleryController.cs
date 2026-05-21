using Lobby.Common.Primitives;
using Lobby.Local.Domain.UseCases.CharacterGallery;

namespace Lobby.Local.Presentation.Controllers
{
    public sealed class CharacterGalleryController
    {
        private readonly FindCharactersUseCase _findCharactersOfRoleUseCase;
        private readonly ICharacterRepository _chracterRepository;

        public CharacterGalleryController(FindCharactersUseCase findCharactersOfRoleUseCase, ICharacterRepository chracterRepository)
        {
            _findCharactersOfRoleUseCase = findCharactersOfRoleUseCase;
            _chracterRepository = chracterRepository;
        }

        public void OpenCharacterSheet(CharacterId characterId)
        {
            UnityEngine.Debug.Log("Open sheet for " + characterId);
        }

        public CharacterId[] LoadAll()
        {
            return _findCharactersOfRoleUseCase.Execute(default);
        }
    }
}
