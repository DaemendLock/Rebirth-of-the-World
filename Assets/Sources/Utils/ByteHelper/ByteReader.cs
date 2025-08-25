using System;
using System.Buffers.Binary;
using System.Text;

namespace Utils.ByteHelper
{
    public class ByteReader
    {
        private readonly byte[] _source;
        private int _position;

        private static Encoding _encoding = Encoding.Unicode;

        public ByteReader(byte[] source) : this(source, 0) { }

        public ByteReader(byte[] source, int offset)
        {
            _source = source;
            _position = offset;
        }

        public bool HasNext => _position < _source.Length;

        public long ReadLong() => BinaryPrimitives.ReadInt64BigEndian(GetMemory(sizeof(long)));

        public ulong ReadULong() => BinaryPrimitives.ReadUInt64BigEndian(GetMemory(sizeof(ulong)));

        public int ReadInt() => BinaryPrimitives.ReadInt32BigEndian(GetMemory(sizeof(int)));

        public uint ReadUInt() => BinaryPrimitives.ReadUInt32BigEndian(GetMemory(sizeof(uint)));

        public ushort ReadUShort() => BinaryPrimitives.ReadUInt16BigEndian(GetMemory(sizeof(ushort)));

        public float ReadFloat()
        {
#if NET7_0_OR_GREATER
            return BinaryPrimitives.ReadSingleBigEndian(GetMemory(sizeof(float)));
#else
            if (BitConverter.IsLittleEndian)
            {
                (_source[_position], _source[_position + sizeof(float) - 1]) = (_source[_position + sizeof(float) - 1], _source[_position]);
                (_source[_position + 1], _source[_position + sizeof(float) - 2]) = (_source[_position + sizeof(float) - 2], _source[_position + 1]);
            }

            return BitConverter.ToSingle(GetMemory(sizeof(float)));
#endif
        }

        public byte ReadByte() => _source[_position++];

        public string ReadString() => _encoding.GetString(GetMemory(ReadUShort()));

        public string ReadZeroTerminatedString(int size) => _encoding.GetString(GetMemory(size)).TrimEnd((char) 0);

        public void Skip(int count) => _position += count;

        public Span<byte> GetMemory(int count)
        {
            _position += count;
            return new(_source, _position - count, count);
        }
    }
}
