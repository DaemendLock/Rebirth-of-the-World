using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Entities.Characters;
using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Domain.UseCases.CharacterGallery
{
    public interface ICharacterRepository
    {
        CharacterProgression Get(CharacterKey id);
        CharacterProgression Get(AccountId owner, CharacterKey id);
    }

    public sealed class FindCharactersUseCase
    {
        private readonly LobbySession _lobbySession;

        public FindCharactersUseCase(LobbySession lobbySession)
        {
            _lobbySession = lobbySession;
        }

        public CharacterKey[] Execute(CharacterFilter characterFilter)
        {
            return new CharacterKey[] { new("katerina"), new("florence") };
        }
    }
}
