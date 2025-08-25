using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

using Utils.ByteHelper;

namespace Client.Combat.Infrastructure.Networking
{
    public class CombatClient
    {
        private const int DefaultBufferSize = 1024;

        private readonly Socket _socket;
        private readonly SocketReader _reader;

        private readonly IPAddress _serverAddres = IPAddress.Parse("127.0.0.1");
        private readonly int _port;

        public CombatClient(int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _reader = new(_socket, DefaultBufferSize);
            _port = port;
        }

        ~CombatClient()
        {
            Disconnect();
        }

        public bool HasMessages => _reader.CanRead;

        public void SendRequest(Request request)
        {
            _socket.Send(request.GetBytes());
        }

        public Task Connect()
        {
            try
            {
                _socket.Connect(_serverAddres, _port);
            }
            catch
            {
                throw;
            }

            return _reader.StartListenToSocket();
        }

        public void Disconnect()
        {
            if (_reader.IsActive == false)
            {
                return;
            }

            _socket.Close();
            _reader.StopListenToSocket();
        }

        public ServerMessage NextMessage() => _reader.DequeueMessage();
    }
}
