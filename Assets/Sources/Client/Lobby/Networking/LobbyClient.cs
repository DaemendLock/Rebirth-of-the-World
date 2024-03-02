using System.Net;
using System.Net.Sockets;

using Client.Lobby.Infrastructure.Adapters;

using UnityEngine;

using Utils.Patterns.Adapters;

namespace Client.Lobby.Networking
{
    public interface Request
    {
        public byte[] GetBytes();
    }

    public class LobbyClient
    {
        private TcpClient _client;

        private IPAddress _serverAddres = IPAddress.Parse("127.0.0.1");
        private int _port = 27122;

        private readonly Adapter<ServerCommand, byte[]> _serverCommandAdapter;

        public LobbyClient(Adapter<ServerCommand, byte[]> serverCommandAdapter)
        {
            _serverCommandAdapter = serverCommandAdapter;
        }

        public void OnResive(byte[] data)
        {
            _serverCommandAdapter.Adapt(data).Perform();
        }

        public void SendRequest(Request request)
        {

        }

        public void UseAdapter(Adapter<ServerCommand, byte[]> serverCommandAdapter)
        {

        }

        public void Connect()
        {
            _client = new TcpClient();
            _client.Connect(_serverAddres, _port);
            _client.Close();
        }
    }
}
