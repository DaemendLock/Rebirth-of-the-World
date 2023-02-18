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

    public enum ResponseType : byte {
        PING_CLIENT = 0,
        ACCOUNT_ACCESS_APPROVED = 1,
        ACCOUNT_DATA = 2,
        REGISTER_APPROVED = 4,
        BAD_REQUEST_DATA = 40,
        CONNECTION_CLOSED = 255,

    }

    public class Response {

        private readonly byte _responseId;
        private readonly ResponseType _type;
        private readonly byte[] _data;
        private bool _handled = false;

        public Response(Response response) {
            _type = response.Type;
            _responseId = response.ResponseId;
            _data = response.Data;
        }

        public Response(byte[] data) {
            _responseId = data[0];
            _type = (ResponseType) data[1];
            _data = data;
        }

        public byte[] Data => _data;

        public ResponseType Type => _type;

        public byte ResponseId => _responseId;

        public bool IsHandled => _handled;

        public void MarkHandled() {
            _handled = true;
        }
    }

    public sealed class AccountAccessResponse : Response {
        private bool success;
        private int id;
        public AccountAccessResponse(byte[] data) : base(data) {
            success = data[2] == 1;
            id = BitConverter.ToInt32(data, 3);
        }
        public AccountAccessResponse(Response response) : base(response) {

            success = response.Data[2] == 1;
        }

        public bool Success => success;

        public int UID => id;
    }

    public sealed class RegisterAccountResponse : Response {
        private bool success;
        public RegisterAccountResponse(byte[] data) : base(data) {
            success = Data[2] == 1;
        }
        public RegisterAccountResponse(Response response) : base(response) {
            success = Data[2] == 1;
        }

        public bool Success => success;
    }

    public sealed class BadResponse : Response {
        public BadResponse(byte[] data) : base(data) {
        }
        public BadResponse(Response response) : base(response) {
        }
    }


}


