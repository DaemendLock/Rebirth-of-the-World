using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.DataStructure
{
    public class ByteReader
    {
        private readonly byte[] _data;
        private int _offset;

        public ByteReader(byte[] data)
        {
            _data = data;
        }

        public ByteReader(byte[] data, int offset)
        {
            _data = data;
            _offset = offset;
        }

        public int ReadInt()
        {
            _offset += sizeof(int);
            return BitConverter.ToInt32(_data, _offset - sizeof(int));
        }

        public ushort ReadUShort()
        {
            _offset+= sizeof(ushort);
            return BitConverter.ToUInt16(_data, _offset);
        }

        public float ReadFloat()
        {
            _offset += sizeof(float);
            return BitConverter.ToSingle(_data, _offset - sizeof(float));
        }

        public byte ReadByte()
        {
            _offset += sizeof(byte);
            return _data[(_offset - sizeof(byte))];
        }
    }
}
