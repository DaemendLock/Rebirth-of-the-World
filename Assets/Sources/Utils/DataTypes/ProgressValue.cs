using System;
using System.Buffers.Binary;

using Utils.ByteHelper;

namespace Utils.DataTypes
{
    public readonly struct ProgressValue
    {
        public readonly byte Level;
        public readonly byte MaxLevel;
        public readonly uint CurrentProgress;
        public readonly uint MaxProgression;

        public ProgressValue(byte level, byte maxLevel, uint currentProgress, uint maxProgression)
        {
            Level = level;
            MaxLevel = maxLevel;
            CurrentProgress = currentProgress;
            MaxProgression = maxProgression;
        }

        public static ProgressValue Parse(ByteReader source)
        {
            byte level = source.ReadByte();
            byte maxLevel = source.ReadByte();
            uint progress = source.ReadUInt();
            uint maxProgress = source.ReadUInt();

            return new(level, maxLevel, progress, maxProgress);
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[10];
            bytes[0] = Level;
            bytes[1] = MaxLevel;
            BinaryPrimitives.WriteUInt32BigEndian(new(bytes, 2, sizeof(int)), CurrentProgress);
            BinaryPrimitives.WriteUInt32BigEndian(new(bytes, 6, sizeof(int)), MaxProgression);

            return bytes;
        }
    }
}
