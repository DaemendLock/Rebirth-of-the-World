using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Networking {

    public enum DataType : byte {
        CONNECTION_CHECK = 0,
        LOGIN_DATA = 1,
        ACCOUNT_DATA = 2,
        LOGOUT = 3,
        REGISTER_DATA = 4,
        JOIN_SCENARIO = 10,
        JOIN_PARTY = 11,
        PARTY_LEADER_CHANGED = 12,
        BAD_DATA = 40,
        DISCONNECT = 255
    }

    public class NetworkModule {
        private const int BUFFER_SIZE = 128;

        public event Action ConnectedToServer;
        public event Action<Response> ServerMessageRecived;
        public event Action DisconnectedFromServer;

        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _inputThread;
        private byte _packetId = 1;
        public Account Account = null;


        public NetworkModule(string server, int port) {
            _client = new TcpClient(server, port);
            _stream = _client.GetStream();
            ListenServerResponse();
            ConnectedToServer?.Invoke();
        }

        public ValueTask SendRequest(Request request)
            => _stream.WriteAsync(request.GetBytes());


        public void Close() {
           // _inputThread.Abort();
            _stream.Close();
            _client.Close();
        }

        public byte NextPacketId {
            get {
                ++_packetId;
                if (_packetId == 0) { ++_packetId; }
                return _packetId;
            }
        }

        private async Task<Response> ReciveResponse() {
            byte[] data = new byte[BUFFER_SIZE];
            await _stream.ReadAsync(data, offset: 0, data.Length);
            return new Response(data);
        }

        private async void ListenServerResponse() {
            while (_client.Connected == true) {
                Response response = await ReciveResponse();
                ServerMessageRecived?.Invoke(response); 
            }
        }

        private abstract class Packet {
            public abstract void Send();
        }
    }

    public class DataPacket {
        private const int DATA_TYPE_ASSIGMENT_POSITION = 0;

        private Header _header;
        public byte[] Data;

        public DataType DataType => (DataType) Data[DATA_TYPE_ASSIGMENT_POSITION];

        public class Header {
            public const int HEADER_SIZE = 2;
            public const int PACKET_ID_POSITION = 0;
            public const int CLIENT_ID_POSITION = 4;

            private readonly byte[] _bytes = new byte[HEADER_SIZE];

            public void ConfugureHeader(ushort packetId, ushort senderId) {
                BitConverter.GetBytes(packetId).CopyTo(_bytes, PACKET_ID_POSITION);
                BitConverter.GetBytes(senderId).CopyTo(_bytes, CLIENT_ID_POSITION);
            }

            public byte[] GetBytes() => _bytes;
        }

        public DataPacket(ushort packetId, ushort senderId, byte[] data) {
            _header = new Header();
            _header.ConfugureHeader(packetId, senderId);
            Data = data;
        }

        public DataPacket(byte[] data) {
            _header = new Header();
            _header.ConfugureHeader(BitConverter.ToUInt16(data, Header.PACKET_ID_POSITION),
                BitConverter.ToUInt16(data, 4));
            Data = new ArraySegment<byte>(data, Header.HEADER_SIZE, data.Length - Header.HEADER_SIZE).ToArray();
        }

        public byte[] GetBytes() {
            byte[] bytes = new byte[Header.HEADER_SIZE + Data.Length];
            _header.GetBytes().CopyTo(bytes, 0);
            Data.CopyTo(bytes, Header.HEADER_SIZE);
            return bytes;
        }
    }

    public interface INetworkPayload {
        byte Id { get;  }

        DataType Type {get;}

        byte[] Data {get;}

        
    }
}
