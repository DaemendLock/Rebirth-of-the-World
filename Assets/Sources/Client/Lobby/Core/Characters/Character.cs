using System;

using UnityEngine;

using Client.Lobby.Core.Common;
using Client.Lobby.Core.Items;

using Utils.DataStructure;
using Utils.DataTypes;
using Client.Lobby.Core.Loading;

namespace Client.Lobby.Core.Characters
{
    public enum GearSlot : byte
    {
        Head,
        Body,
        Legs,
        MainHand,
        OffHand,
        Ring1,
        Ring2,
        Consumable1,
        Consumable2,
    }

    public class CharacherGear
    {
        private readonly Item[] _gear;

        public Item Head => _gear[0];
        public Item Body => _gear[1];
        public Item Legs => _gear[2];
        public Item Weapon1 => _gear[3];
        public Item Weapon2 => _gear[4];
        public Item Ring1 => _gear[5];
        public Item Ring2 => _gear[6];
        public Item Utility1 => _gear[7];
        public Item Utility2 => _gear[8];
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
        public StatsTable BaseStats { get; }

        public StatsTable BonusStats { get; }
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

    public class Character : UpdateableModel
    {
        public event Action Updated;

        private DelaiedData<Character> _delaiedData;

        private CharacterAppearance _appearence;
        private CharacterProgression _progression;
        private CharacherGear _gear;
        private CharacterStats _stats;
        private CharacterSpells _spells;

        public Character(CharacterInfo info, CharacterAppearance appearence)
        {
            Info = info;
            _appearence = appearence;
        }

        public Character(CharacterInfo info, CharacterAppearance appearence, CharacterProgression progression, CharacherGear gear, CharacterStats stats, CharacterSpells spells)
        {
            Info = info;
            _appearence = appearence;
            _progression = progression;
            _gear = gear;
            _stats = stats;
            _spells = spells;
        }

        public DelaiedData<Character> DelaiedData { set => _delaiedData = value; }

        public CharacterInfo Info { get; }

        public CharacterAppearance Appearance
        {
            get => _appearence;
        }

        public CharacterProgression Progression
        {
            get => _progression ??= _delaiedData.GetValue()._progression;
        }

        public CharacherGear Gear
        {
            get => _gear ??= _delaiedData.GetValue()._gear;
        }

        public CharacterSpells Spells
        {
            get => _spells ??= _delaiedData.GetValue()._spells;
        }

        public CharacterStats Stats
        {
            get => _stats ??= _delaiedData.GetValue()._stats;
        }

        public void Update(Character character)
        {
            _progression = character._progression;
            _gear = character._gear;
            _stats = character._stats;
            _spells = character._spells;

            Updated?.Invoke();
        }
    }

    public class Spell
    {
        public Spell(int id, Sprite sprite)
        {
            Id = id;
            Icon = sprite;
        }

        public event Action Updated;

        public int Id { get; set; }
        public Sprite Icon { get; set; }
        public string Name { get; set; }
    }
}
