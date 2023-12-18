using Data.Characters.Lobby;
using Data.Items;
using System;
using System.IO;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Data.Characters
{
    [Serializable]
    public class CharacterState
    {
        public int CharacterId;

        public int ViewSet;
        public int ActiveSpec;

        public readonly SpellId[] Spells;
        public readonly ItemId[] Gear;

        public CharacterState(int charcterId, int viewSet, int activeSpec, ProgressValue level, ProgressValue affection, SpellId[] spells, ItemId[] gear)
        {
            CharacterId = charcterId;
            ViewSet = viewSet;
            ActiveSpec = activeSpec;
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

        public static CharacterState Parse(byte[] data)
        {
            int characterId;

            int viewSet;
            int activeSpec;

            ProgressValue level;
            ProgressValue affection;

            SpellId[] spells;
            ItemId[] gear;

            MemoryStream stream = new(data);

            using (BinaryReader source = new(stream))
            {
                characterId = source.ReadInt32();
                viewSet = source.ReadInt32();
                activeSpec = source.ReadInt32();

                level = ProgressValue.Parse(source);
                affection = ProgressValue.Parse(source);

                spells = new SpellId[source.ReadByte()];

                for (int i = 0; i < spells.Length; i++)
                {
                    spells[i] = (SpellId) source.ReadInt32();
                }

                gear = new ItemId[source.ReadByte()];

                for (int i = 0; i < gear.Length; i++)
                {
                    gear[i] = (ItemId) source.ReadInt32();
                }
            }

            stream.Dispose();

            return new CharacterState(characterId, viewSet, activeSpec, level, affection, spells, gear);
        }

        public byte[] GetBytes()
        {
            using MemoryStream target = new();

            target.Write(BitConverter.GetBytes(CharacterId));
            target.Write(BitConverter.GetBytes(ViewSet));
            target.Write(BitConverter.GetBytes(ActiveSpec));
            target.Write(Level.GetBytes());
            target.Write(Affection.GetBytes());

            target.WriteByte((byte) Spells.Length);

            for (int i = 0; i < Spells.Length; i++)
            {
                target.Write(BitConverter.GetBytes(Spells[i]));
            }

            target.WriteByte((byte) Gear.Length);

            for (int i = 0; i < Gear.Length; i++)
            {
                target.Write(BitConverter.GetBytes(Gear[i]));
            }

            return target.ToArray();
        }
    }
}

