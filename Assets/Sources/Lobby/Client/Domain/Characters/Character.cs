using System;

using UnityEngine;

using Client.Lobby.Domain.Common;
using Client.Lobby.Domain.Items;

using Utils.DataStructure;
using Utils.DataTypes;
using System.Threading.Tasks;
using Utils.Patterns.DataProviders;
using Utils.ThrowHepler;

namespace Client.Lobby.Domain.Characters
{
    public class CharacterGear : IEquipmentInfo
    {
        private readonly Item[] _gear;

        public CharacterGear(ItemId[] items)
        {

        }

        public Item GetItem(GearSlot slot) => _gear[(int) slot];
    }

    public class CharacterProgression
    {
        public CharacterProgression(ProgressValue level, ProgressValue affection)
        {
            Level = level;
            Affection = affection;
        }

        public ProgressValue Level { get; }
        public ProgressValue Affection { get; }
    }

    public class CharacterSpells
    {
        private readonly Spell[] _spells;

        public CharacterSpells(Spell[] spells)
        {
            _spells = spells;
        }

        public int Count => _spells.Length;

        public Spell GetSpell(int index)
        {
            return _spells[index];
        }
    }

    public class CharacterStats
    {
        //public StatsTable BaseStats { get; }

        //public StatsTable BonusStats { get; }
    }

    public class CharacterInfo
    {
        public CharacterInfo(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }

    public class CharacterAppearance
    {
        public CharacterAppearance(Sprite cardImage)
        {
            CardImage = cardImage;
        }

        public Sprite CardImage { get; }
    }

    public class Character : IUpdateableModel, ILoadableModel
    {
        public event Action Updated;

        private readonly AsyncDataProvider<int, Character> _dataProvider;

        private CharacterProgression _progression;
        private IEquipmentInfo _gear;
        private CharacterStats _stats;
        private CharacterSpells _spells;

        private CharacterAppearance _appearance;

        public Character(CharacterInfo info, CharacterAppearance appearence, AsyncDataProvider<int, Character> fullDataProvider)
        {
            Info = info;
            _appearance = appearence;
            _dataProvider = fullDataProvider;
            IsLoaded = false;
        }

        public Character(CharacterInfo info, CharacterAppearance appearence, CharacterProgression progression, IEquipmentInfo gear, CharacterStats stats, CharacterSpells spells)
        {
            ThrowHepler.ArgumentNullException(info, appearence, progression, gear, stats, spells);

            Info = info;
            _appearance = appearence;
            _progression = progression;
            _gear = gear;
            _stats = stats;
            _spells = spells;
            IsLoaded = true;
        }

        public bool IsLoaded { get; private set; }

        public CharacterInfo Info { get; }

        public CharacterAppearance Appearance => _appearance;

        public CharacterProgression Progression => _progression;

        public IEquipmentInfo Gear => _gear;

        public CharacterSpells Spells => _spells;

        public CharacterStats Stats => _stats;

        public async Task Load()
        {
            if (IsLoaded)
            {
                return;
            }

            await _dataProvider.GetValue(Info.Id);
        }

        public void Update(Character character)
        {
            _appearance = character._appearance;

            if (character.IsLoaded)
            {
                _progression = character._progression;
                _gear = character._gear;
                _stats = character._stats;
                _spells = character._spells;
                IsLoaded = true;
                _dataProvider.ProvideData(character.Info.Id, character);
            }

            Updated?.Invoke();
        }
    }

    public class Spell
    {
        public event Action Updated;

        public Spell(int id, Sprite sprite)
        {
            Id = id;
            Icon = sprite;
        }

        public int Id { get; set; }
        public Sprite Icon { get; set; }
        public string Name { get; set; }
    }
}
