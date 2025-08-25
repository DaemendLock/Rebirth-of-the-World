using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

using UtilsUnity.ByteHelper;
using Utils.ByteHelper;
using Utils.Networking;

namespace UtilsUnity.Networking
{
    public class SocketClient : IClient
    {
        private readonly Socket _socket;
        private readonly SocketReader _reader;

        private readonly EndPoint _endPoint;

        public SocketClient(Socket socket, EndPoint endPoint)
        {
            _socket = socket;
            _endPoint = endPoint;
            _reader = new(_socket);
        }

        ~SocketClient()
        {
            Disconnect();
        }

        public bool HasMessages => _reader.CanRead;

        public void SendRequest(Request request) => _socket.Send(request.GetBytes());

        public Utils.ByteHelper.ServerMessage NextMessage() => _reader.DequeueMessage();

        public Task Connect()
        {
            try
            {
                _socket.Connect(_endPoint);
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
    }
}
