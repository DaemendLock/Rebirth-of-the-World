using System;
using System.IO;

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

        public static ProgressValue Parse(BinaryReader source)
        {
            byte level = source.ReadByte();
            byte maxLevel = source.ReadByte();
            uint progress = source.ReadUInt32();
            uint maxProgress = source.ReadUInt32();

            return new(level, maxLevel, progress, maxProgress);
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[10];
            bytes[0] = Level;
            bytes[1] = MaxLevel;
            BitConverter.GetBytes(CurrentProgress).CopyTo(bytes, 2);
            BitConverter.GetBytes(MaxProgression).CopyTo(bytes, 6);

            return bytes;
        }
    }
}
