using Client.Lobby.Infrastructure.Factories;

using Client.Lobby.Core.Accounts;
using Client.Lobby.Core.Characters;

using Data.Localization;
using Utils.Patterns.Adapters;

namespace Client.Lobby.Infrastructure.Adapters
{
    public class CharacterDataAdapter : Adapter<Character, Data.Characters.Character>
    {
        private readonly Account _account;

        public CharacterDataAdapter(Account account)
        {
            _account = account;
        }

        public Character Adapt(Data.Characters.Character character)
        {
            CharacterData data = _account.GetCharacterData(character.Id);

            CharacterInfo info = new(character.Id, Localization.GetValue(character.Name));
            CharacterAppearance appearance = new(character.Npc.GetCharacterCard(data.ActiveViewSet));

            Character result = new(info, appearance);
            result.DelaiedData = new DelaidFullCharacterData(_account, result);
            return result;
        }
    }
}
