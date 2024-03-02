using Client.Lobby.Core.Characters;
using System.Collections.Generic;

namespace Client.Lobby.Infrastructure.Providers
{
    public interface DefaultCharactersProvider
    {
        public List<Character> GetCharacters();
    }
}
