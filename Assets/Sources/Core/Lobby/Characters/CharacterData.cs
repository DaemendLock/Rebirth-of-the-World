using Data.Characters.Lobby;
using System;
using System.IO;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Lobby.Characters
{
    [Serializable]
    public class CharacterData
    {
        public ProgressValue Level { get; private set; }
        public ProgressValue Affection { get; private set; }

        public readonly SpellId[] Spells;

        public readonly StatsTable DefaultStats;

        public readonly ItemId[] Gear;

        public CharacterData(ProgressValue level, ProgressValue affection, SpellId[] spells)
        {
            Level = level;
            Affection = affection;
            Spells = spells;
            DefaultStats = StatsTable.UNIT_DEFAULT;
            Gear = new ItemId[0];
        }

        public StatsTable EvaluateStats()
        {
            return DefaultStats;
        }

        public static CharacterData Parse(byte[] data, int start)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes()
        {
            byte[] result;

            int size = 4 * (sizeof(int) + sizeof(byte)) + sizeof(int) + (Spells.Length * sizeof(int)) + sizeof(int) + (sizeof(int) * Gear.Length);

            using (MemoryStream stream = new MemoryStream())
            {

            }

            throw new NotImplementedException();
        }
    }
}

