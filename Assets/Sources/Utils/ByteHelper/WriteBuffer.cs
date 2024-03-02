using System;

namespace Utils.ByteHelper
{
    public class WriteBuffer
    {
        private byte[] _buffer;
        private int _cursor;

        public WriteBuffer(int capacity)
        {
            _buffer = new byte[capacity];
        }

        public int Capacity => _buffer.Length;

        public void Write(ushort value)
        {
            BitConverter.GetBytes(value).CopyTo(_buffer, _cursor);
            _cursor += sizeof(ushort);
        }

        public void Write(int value)
        {
            BitConverter.GetBytes(value).CopyTo(_buffer, _cursor);
            _cursor += sizeof(int);
        }

        public void Write(float value)
        {
            BitConverter.GetBytes(value).CopyTo(_buffer, _cursor);
            _cursor += sizeof(float);
        }

        public void Clear() => _cursor = 0;

        public void Resize(int newCapacity)
        {
            if (newCapacity < _buffer.Length)
            {
                return;
            }

            byte[] newBuffer = new byte[newCapacity];
            _buffer.CopyTo(newBuffer, 0);
            _buffer = newBuffer;
        }
    }
}
