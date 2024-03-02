using System;

namespace Utils.ByteHelper
{
    public class ByteReader
    {
        private readonly byte[] _data;
        private int _offset;

        public ByteReader(byte[] data) : this(data, 0) { }

        public ByteReader(byte[] data, int offset)
        {
            _data = data;
            _offset = offset;
        }

        public bool HasNext => _offset < _data.Length;

        public long ReadLong()
        {
            _offset += sizeof(long);
            return BitConverter.ToInt64(_data, _offset - sizeof(long));
        }

        public int ReadInt()
        {
            _offset += sizeof(int);
            return BitConverter.ToInt32(_data, _offset - sizeof(int));
        }

        public ushort ReadUShort()
        {
            _offset += sizeof(ushort);
            return BitConverter.ToUInt16(_data, _offset - sizeof(ushort));
        }

        public float ReadFloat()
        {
            _offset += sizeof(float);
            return BitConverter.ToSingle(_data, _offset - sizeof(float));
        }

        public byte ReadByte()
        {
            _offset += sizeof(byte);
            return _data[_offset - sizeof(byte)];
        }
    }
}
