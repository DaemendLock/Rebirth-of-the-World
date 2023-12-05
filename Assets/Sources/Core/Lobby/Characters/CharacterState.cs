using Data.Characters.Lobby;
using Data.Items;
using System;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Lobby.Characters
{
    [Serializable]
    public class CharacterState
    {
        public int ViewSet;
        public int ActiveSpec;

        public readonly SpellId[] Spells;
        public readonly ItemId[] Gear;

        public CharacterState(ProgressValue level, ProgressValue affection, SpellId[] spells, ItemId[] gear)
        {
            Level = level;
            Affection = affection;
            Spells = spells;
            Gear = gear;
        }

        public ProgressValue Level { get; private set; }
        public ProgressValue Affection { get; private set; }

        public StatsTable GetGearStats()
        {
            StatsTable result = new StatsTable();

            foreach (ItemId item in Gear)
            {
                if (item == -1)
                {
                    continue;
                }

                result += Item.GetGear(item).Stats;
            }

            return result;
        }

        public SpellId[] GetGearSpells()
        {
            return new SpellId[0];
        }

        public static CharacterState Parse(byte[] data, int start)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }
}

