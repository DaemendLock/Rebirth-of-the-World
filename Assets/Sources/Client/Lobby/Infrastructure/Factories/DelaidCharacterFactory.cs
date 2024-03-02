using Client.Lobby.Core.Accounts;
using Client.Lobby.Core.Characters;
using Client.Lobby.Core.Loading;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class DelaidCharacterFactory : Factory<DelaiedData<Character>, Character>
    {
        private Account _account;

        public DelaidCharacterFactory(Account account)
        {
            _account = account;
        }

        public DelaiedData<Character> Create(Character character)
        {
            return new DelaidFullCharacterData(_account, character);
        }
    }

    public class DelaidFullCharacterData : DelaiedData<Character>, Factory<Character>
    {
        private readonly Account _account;
        private readonly Character _character;
        private bool _loaded = false;

        public DelaidFullCharacterData(Account account, Character character)
        {
            _account = account;
            _character = character;
        }

        public Character GetValue()
        {
            if (_loaded == false)
            {
                _character.Update(Create());
                _loaded = true;
            }

            return _character;
        }

        public Character Create()
        {
            CharacterData data = _account.GetCharacterData(_character.Info.Id);
            Data.Characters.Character @base = Data.Characters.Character.Get(_character.Info.Id);

            CharacterProgression progression = new CharacterProgression(data.Level, data.Affection);
            CharacherGear gear = new CharacherGear();
            CharacterStats stats = new CharacterStats();

            int[] spellIds = @base.GetSpells(data.ActiveSpec);
            Spell[] spellModels = new Spell[spellIds.Length];

            for (int i = 0; i < spellIds.Length; i++)
            {
                spellModels[i] = new(spellIds[i], @base.Npc.GetSpellIcon(data.ActiveViewSet, (Utils.DataTypes.SpellId) spellIds[i]));
            }

            CharacterSpells spells = new(spellModels);

            Character result = new(_character.Info, _character.Appearance, progression, gear, stats, spells);
            return result;
        }
    }
}
