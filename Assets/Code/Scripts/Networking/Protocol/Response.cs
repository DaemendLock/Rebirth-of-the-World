using System;

namespace Networking {
    public enum RequestFail : byte {
        SUCCES = 0,
        FAIL = 1
    }

    public enum AuthinficationFail : byte {
        SUCCES = 0,
        AUTH_FAIL = 1,
        CONNECTION_FAILED = 2
    }

    public class Response : INetworkPayload {
        private const int RESPONSE_ID_INDEX = 0;
        private const int TYPE_ID_INDEX = 1;
        public const int HEADER_SIZE = 2;

        private readonly byte _responseId;
        private readonly DataType _type;
        private readonly byte[] _data;

        public Response(Response response) {
            _type = response.Type;
            _responseId = response.ResponseId;
            _data = response.Data;
        }

        public Response(byte[] data) {
            _responseId = data[RESPONSE_ID_INDEX];
            _type = (DataType) data[TYPE_ID_INDEX];
            _data = new ArraySegment<byte>(data, HEADER_SIZE, data.Length - HEADER_SIZE).ToArray();
        }

        public byte[] Data => _data;

        public DataType Type => _type;

        public byte Id => _responseId;

        public byte ResponseId => _responseId;

    }

    public sealed class AccountAccessResponse : Response {
        private const int SUCCESS_BYTE_INDEX = 0;
        private const int UID_START_BYTE = 1;

        private bool _success;
        private int _uid;

        public AccountAccessResponse(byte[] bytes) : base(bytes) {
            _success = Data[SUCCESS_BYTE_INDEX] == 1;
            _uid = BitConverter.ToInt32(Data, UID_START_BYTE);
        }
        public AccountAccessResponse(Response response) : base(response) {
            _success = response.Data[SUCCESS_BYTE_INDEX] == 1;
            _uid = BitConverter.ToInt32(response.Data, UID_START_BYTE);
        }

        public bool Success => _success;

        public int UID => _uid;
    }

    public sealed class RegisterAccountResponse : Response {
        private const int SUCCESS_BYTE_INDEX = 0;

        private readonly bool _success;

        public RegisterAccountResponse(byte[] data) : base(data) {
            _success = Data[SUCCESS_BYTE_INDEX + HEADER_SIZE] == 1;
        }
        public RegisterAccountResponse(Response response) : base(response) {
            _success = Data[SUCCESS_BYTE_INDEX] == 1;
        }

        public bool Success => _success;
    }

    public sealed class BadResponse : Response {
        public BadResponse(byte[] data) : base(data) {
        }
        public BadResponse(Response response) : base(response) {
        }
    }


}


