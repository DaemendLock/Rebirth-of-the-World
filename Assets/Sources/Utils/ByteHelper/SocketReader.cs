using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Utils.ByteHelper
{
    public readonly struct ServerMessage
    {
        public readonly int Id;
        public readonly byte[] Data;

        public ServerMessage(int id, byte[] data)
        {
            Id = id;
            Data = data;
        }
    }

    public class SocketReader
    {
        public event Action<ServerMessage> Received;

        private const int DefaultBufferSize = 512;
        private const int DefaultMessageQueueSize = 256;

        private readonly byte[] _buffer;
        private readonly Socket _source;
        private readonly Queue<ServerMessage> _messages;

        public SocketReader(Socket source) : this(source, DefaultBufferSize) { }

        public SocketReader(Socket source, int bufferSize) : this(source, new byte[bufferSize > 0 ? bufferSize : DefaultBufferSize], new Queue<ServerMessage>(DefaultMessageQueueSize))
        {
        }

        private SocketReader(Socket socket, byte[] buffer, Queue<ServerMessage> memory)
        {
            _source = socket;
            _buffer = buffer;
            _messages = memory;
        }

        public bool IsActive { get; private set; }

        public bool CanRead => _messages.Count > 0;

        public Task StartListenToSocket()
        {
            IsActive = true;
            return Task.Run(ListenToSocket);
        }

        public void StopListenToSocket()
        {
            IsActive = false;
        }

        public ServerMessage DequeueMessage() => _messages.Dequeue();

        private void ListenToSocket()
        {
            while (IsActive)
            {
                ServerMessage nextMessage = ReadNextMessage();
                _messages.Enqueue(nextMessage);
                Received?.Invoke(nextMessage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ServerMessage ReadNextMessage()
        {
            int cursor = 0;
            byte[] bytes = null;

            do
            {
                ReciveNext(ref cursor, ref bytes);
            } while (_source.Available > 0);

            return new ServerMessage(0, bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReciveNext(ref int cursor, ref byte[] bytes)
        {
            int messageSize = _source.Receive(_buffer);
            Array.Resize(ref bytes, cursor + messageSize);
            CopyTo(_buffer, bytes, cursor, messageSize);
            cursor += messageSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CopyTo(byte[] source, byte[] target, int start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                target[start + i] = source[i];
            }
        }
    }
}
