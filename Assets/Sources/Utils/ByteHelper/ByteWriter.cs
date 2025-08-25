using System;
using System.Buffers.Binary;
using System.Text;

using Utils.DataTypes;

namespace Utils.ByteHelper
{
    public interface ByteWritable
    {
        void WriteBytes(ByteWriter target);
    }

    public class ByteWriter
    {
        private int _position;

        private byte[] _buffer;

        public ByteWriter() : this(Array.Empty<byte>()) { }

        public ByteWriter(ByteWriter memory) : this(memory._buffer) { }

        public ByteWriter(int capacity) : this(new byte[capacity]) { }

        private ByteWriter(byte[] buffer)
        {
            _buffer = buffer;
            _position = 0;
        }

        public void Clear()
        {
            _position = 0;
        }

        public void Write(byte value) => GetMemory(1)[0] = value;

        public void Write(short value) => BinaryPrimitives.WriteInt16BigEndian(GetMemory(sizeof(short)), value);

        public void Write(ushort value) => BinaryPrimitives.WriteUInt16BigEndian(GetMemory(sizeof(ushort)), value);

        public void Write(int value) => BinaryPrimitives.WriteInt32BigEndian(GetMemory(sizeof(int)), value);

        public void Write(uint value) => BinaryPrimitives.WriteUInt32BigEndian(GetMemory(sizeof(uint)), value);

        public void Write(long value) => BinaryPrimitives.WriteInt64BigEndian(GetMemory(sizeof(long)), value);

        public void Write(ulong value) => BinaryPrimitives.WriteUInt64BigEndian(GetMemory(sizeof(ulong)), value);

        public void Write(float value)
        {
#if NET7_0_OR_GREATER
            BinaryPrimitives.WriteSingleBigEndian(GetMemory(sizeof(float)), value);
#else
            BitConverter.TryWriteBytes(GetMemory(sizeof(float)), value);

            if (BitConverter.IsLittleEndian)
            {
                (_buffer[_position - 1], _buffer[_position - sizeof(float)]) = (_buffer[_position - 1], _buffer[_position - sizeof(float)]);
                (_buffer[_position - 2], _buffer[_position - sizeof(float) + 1]) = (_buffer[_position - 2], _buffer[_position - sizeof(float) + 1]);
            }
#endif
        }

        public void Write(string value)
        {
            Encoding encoding = Encoding.Unicode;
            byte[] bytes = encoding.GetBytes(value);

            if (bytes.Length > ushort.MaxValue)
            {
                throw new OverflowException($"Serialized string length is too big. Must be less than {ushort.MaxValue} bytes.");
            }

            Write((ushort) bytes.Length);

            Span<byte> memory = GetMemory(bytes.Length);
            bytes.CopyTo(memory);
        }

        public void Write(byte[] value)
        {
            value.CopyTo(GetMemory(value.Length));
        }

        public void Write(ByteWritable value)
        {
            value.WriteBytes(this);
        }

        public void Write(ProgressValue value)
        {
            Write(value.Level);
            Write(value.MaxLevel);
            Write(value.CurrentProgress);
            Write(value.MaxProgression);
        }

        public void Skip(int count)
        {
            while (_position + count > _buffer.Length)
            {
                Array.Resize(ref _buffer, _position + count);
            }

            _position += count;
        }

        public byte[] GetBuffer() => _buffer;

        public byte[] ToArray()
        {
            byte[] result = new byte[_position];

            Array.Copy(_buffer, result, _position);

            return result;
        }

        private Span<byte> GetMemory(int count)
        {
            while (_position + count > _buffer.Length)
            {
                Array.Resize(ref _buffer, _position + count);
            }

            _position += count;
            return new Span<byte>(_buffer, _position - count, count);
        }
    }
}
