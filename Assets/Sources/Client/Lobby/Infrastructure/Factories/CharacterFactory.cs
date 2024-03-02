using Client.Lobby.Core.Accounts;
using Client.Lobby.Core.Characters;
using Client.Lobby.Infrastructure.Providers;

using Data.Localization;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class CharacterFactory : Factory<Character, CharacterData>
    {
        private readonly Account _account;
        private readonly LoadingService<Data.Characters.Character> _loader;

        public CharacterFactory(Account account)
        {
            _account = account;
        }

        public Character Create(CharacterData data)
        {
            Data.Characters.Character character = _loader.Load(data.Id);

            CharacterInfo info = new(character.Id, Localization.GetValue(character.Name));
            CharacterAppearance appearance = new(character.Npc.GetCharacterCard(data.ActiveViewSet));

            Character result = new(info, appearance);
            result.DelaiedData = new DelaidFullCharacterData(_account, result);
            return result;
        }
    }
}
