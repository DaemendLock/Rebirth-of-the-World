using System.Collections.Generic;
using System.Linq;

using Client.Lobby.Domain.Accounts;
using Client.Lobby.Domain.Characters;
using Client.Lobby.Infrastructure.Networking.Requests;

using Utils.Patterns.Adapters;
using Utils.Patterns.DataProviders;

namespace Client.Lobby.Infrastructure.Providers
{
    public interface CharactersProvider
    {
        public List<Character> GetCharacters();
    }

    public class CharacterFullDataProvider : AsyncDataProvider<int, Character>
    {
        private readonly UtilsUnity.Networking.IClient _lobbyClient;
        private readonly int _accountId;

        public CharacterFullDataProvider(UtilsUnity.Networking.IClient lobbyClient, int accountId)
        {
            _lobbyClient = lobbyClient;
            _accountId = accountId;
        }

        protected override void OnValueRequested(int key)
        {
            _lobbyClient.SendRequest(new AccountDataRequest(_accountId, AccountDataType.CharacterData, (int) CharacterDataParams.FullData, key));
        }
    }

    public class CharacterPartialDataProvider : AsyncDataProvider<int, Character>
    {
        private readonly UtilsUnity.Networking.IClient _lobbyClient;
        private readonly int _accountId;

        public CharacterPartialDataProvider(UtilsUnity.Networking.IClient lobbyClient, int accountId)
        {
            _lobbyClient = lobbyClient;
            _accountId = accountId;
        }

        protected override void OnValueRequested(int key) 
        {
            _lobbyClient.SendRequest(new AccountDataRequest(_accountId, AccountDataType.CharacterData, (int) CharacterDataParams.ShortData, key));
        }
    }

    public class DefaultCharactersProvider : CharactersProvider
    {
        private readonly List<Character> _characters;

        public DefaultCharactersProvider(IEnumerable<Data.Characters.Character> characters, Adapter<Character, Data.Characters.Character> adapter)
        {
            _characters = characters.Select(character => adapter.Adapt(character)).ToList();
        }

        public List<Character> GetCharacters() => _characters;
    }
}
