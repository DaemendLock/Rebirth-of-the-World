using Core.Combat.Gear;
using System.IO;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Utils.Serializer
{
    internal class ItemSerializer
    {
        public static byte[] Serialize(Gear data)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (BinaryWriter buffer = new BinaryWriter(memoryStream))
            {
                buffer.Write(data.Id);
                buffer.Write(data.HasSpell);
                buffer.Write(data.Spell);
                SerializeStats(buffer, data.Stats);
            }

            byte[] result = memoryStream.ToArray();
            memoryStream.Dispose();

            return result;
        }

        public static Gear Deserialize(byte[] data)
        {
            int id;
            bool hasSpell;
            int spellId;
            StatsTable stats;

            MemoryStream memoryStream = new MemoryStream(data);

            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
            {
                id = binaryReader.ReadInt32();
                hasSpell = binaryReader.ReadBoolean();
                spellId = binaryReader.ReadInt32();
                stats = DeserializeStats(binaryReader);
            }

            memoryStream.Dispose();

            return new Gear((ItemId) id, stats, hasSpell, (SpellId) spellId);
        }

        public static void SerializeStats(BinaryWriter target, StatsTable statsTable)
        {
            for (int i = 0; i < StatsTable.STATS_COUNT; i++)
            {
                target.Write(statsTable[i].BaseValue);
                target.Write(statsTable[i].Percent);
            }
        }

        public static StatsTable DeserializeStats(BinaryReader source)
        {
            StatsTable result = new();

            for (int i = 0; i < StatsTable.STATS_COUNT; i++)
            {
                float baseValue = source.ReadSingle();
                float percent = source.ReadSingle();
                result[i] = new PercentModifiedValue(baseValue, percent);
            }

            return result;
        }
    }
}
