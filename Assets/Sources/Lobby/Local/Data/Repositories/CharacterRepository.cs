using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities.Characters;
using Lobby.Local.Domain.UseCases.CharacterGallery;

namespace Lobby.Local.Data.Repositories
{
    public sealed class CharacterRepository : ICharacterRepository
    {
        public CharacterProgression Get(CharacterKey id) => new(id, default, true);

        public CharacterProgression Get(AccountId owner, CharacterKey id) => new(id, default, false);
    }
}
