using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities.Characters;
using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Domain.UseCases.CharacterGallery
{
    public interface ICharacterRepository
    {
        Character Get(CharacterId id);
        Character Get(AccountId owner, CharacterId id);
    }

    public sealed class FindCharactersUseCase
    {
        public CharacterId[] Execute(CharacterFilter characterFilter)
        {
            return new CharacterId[] { new(0), new(1) };
        }
    }
}
