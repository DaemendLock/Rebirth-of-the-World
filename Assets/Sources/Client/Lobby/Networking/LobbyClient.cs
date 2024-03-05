using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Client.Lobby.Infrastructure.Adapters;

using UnityEngine;

using Utils.ByteHelper;
using Utils.Patterns.Adapters;

namespace Client.Lobby.Networking
{
    public interface Request
    {
        public byte[] GetBytes();
    }

    public class LobbyClient
    {
        private readonly TcpClient _client;
        private readonly SocketReader _reader;

        private readonly IPAddress _serverAddres = IPAddress.Parse("127.0.0.1");
        private readonly int _port = 27122;

        private readonly Adapter<ServerCommand, byte[]> _serverCommandAdapter;

        public LobbyClient(Adapter<ServerCommand, byte[]> serverCommandAdapter)
        {
            _serverCommandAdapter = serverCommandAdapter;
            _client = new TcpClient();
            _reader = new(_client.Client);
        }

        ~LobbyClient()
        {
            _client.Close();
            Debug.Log("Connection closed");
        }

        public bool HasMessages => _reader.CanRead;

        public void OnResive(byte[] data)
        {
            _serverCommandAdapter.Adapt(data).Perform();
        }

        public void SendRequest(Request request)
        {

        }

        public Task Connect()
        {
            _client.Connect(_serverAddres, _port);
            _reader.Received += (ctx) => Debug.Log($"New message. Size: {ctx.Data.Length}");
            Task listenTask = _reader.StartListenToSocket();
            return listenTask;
        }

        public ServerCommand HandleNext() => _serverCommandAdapter.Adapt(_reader.DequeueMessage().Data);
    }
}
