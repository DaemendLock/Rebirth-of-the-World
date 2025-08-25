using Client.Lobby.Domain.Characters;

using Data.Localization;

using Utils.Patterns.Adapters;

namespace Client.Lobby.Infrastructure.Adapters
{
    public class DefaultCharacterDataAdapter : Adapter<Character, Data.Characters.Character>
    {
        public Character Adapt(Data.Characters.Character character)
        {
            Character result;

            CharacterInfo info = new(character.Id, Localization.GetValue(character.Name));
            CharacterAppearance appearance = new(character.Npc.GetCharacterCard(0));
            result = new(info, appearance, null);

            return result;
        }
    }
}
