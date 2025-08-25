using Client.Lobby.Domain.Accounts;
using Client.Lobby.Domain.Characters;
using Client.Lobby.Infrastructure.Providers;

using Data.Localization; 

using Utils.DataTypes;
using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class CharacterFactory : Factory<Character, CharacterData>
    {
        private readonly UtilsUnity.Networking.IClient _lobbyClient;

        public CharacterFactory(UtilsUnity.Networking.IClient lobbyClient)
        {
            _lobbyClient = lobbyClient;
        }

        public Character Create(CharacterData data)
        {
            Data.Characters.Character @base = Data.Characters.Character.Get(data.CharacterId);

            CharacterInfo info = new(data.CharacterId, Localization.GetValue(@base.Name));
            CharacterAppearance appearance = new(@base.Npc.GetCharacterCard(data.ActiveViewSet));

            if (data.Full == false)
            {
                return new(info, appearance, new CharacterFullDataProvider(_lobbyClient, data.AccountId));
            }

            CharacterProgression progression = new(data.Level, data.Affection);
            CharacterStats stats = new();
            IEquipmentInfo gear = new CharacterGear(data.Items);

            int[] spellIds = @base.GetSpells(data.ActiveSpec);
            Spell[] spellModels = new Spell[spellIds.Length];

            for (int i = 0; i < spellIds.Length; i++)
            {
                spellModels[i] = new(spellIds[i], @base.Npc.GetSpellIcon(data.ActiveViewSet, (SpellId) spellIds[i]));
            }

            CharacterSpells spells = new(spellModels);

            return new(info, appearance, progression, gear, stats, spells);
        }
    }
}
