using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities.Characters;
using Lobby.Local.Domain.UseCases.CharacterGallery;

namespace Lobby.Local.Data.Repositories
{
    public sealed class CharacterRepository : ICharacterRepository
    {
        public Character Get(CharacterId id) => new(id, default, true);

        public Character Get(AccountId owner, CharacterId id) => new(id, default, false);
    }
}
