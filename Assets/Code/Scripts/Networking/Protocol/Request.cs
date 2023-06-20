using System;
using System.Text;

namespace Networking {
    public class Request : INetworkPayload {
        private const int ID_BYTE_INDEX = 0;
        private const int TYPE_BYTE_INDEX = 1;
        private const int HEADER_SIZE = 2;
        
        public readonly byte _Id;
        private readonly DataType _type;
        private readonly byte[] _data;  

        private Request(byte requestId, DataType type, byte[] data) {
            _Id = requestId;
            _type = type;
            _data = data;
        }

        public byte Id => _Id;

        public DataType Type => _type;

        public byte[] Data => _data;

        public byte[] GetBytes() {
            byte[] bytes = new byte[HEADER_SIZE + _data.Length];
            bytes[ID_BYTE_INDEX] = _Id;
            bytes[TYPE_BYTE_INDEX] = (byte)_type;
            _data.CopyTo(bytes, HEADER_SIZE);
            return bytes;
        }

        public static Request MakeAccountAccessRequest(byte requestId, DataType type, string login, string password) {
            byte[] data = new byte[login.Length + password.Length + 1];
            Encoding.ASCII.GetBytes(login).CopyTo(data, 0);
            Encoding.ASCII.GetBytes(password).CopyTo(data, login.Length + 1);
            return new(requestId, type, data);
        }

        public static Request MakeLogoutRequest(byte requestId) {
            return new(requestId, DataType.DISCONNECT, new byte[0]);
        }

        public static Request MakePingRequest(byte requestId) => new Request(requestId, DataType.CONNECTION_CHECK, BitConverter.GetBytes(DateTime.UnixEpoch.Millisecond));

        public static Request MakeLogoutRequest() => new(0, DataType.LOGOUT, new byte[0]);
    }


}