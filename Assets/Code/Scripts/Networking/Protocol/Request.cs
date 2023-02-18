namespace Networking {
    public enum RequestType : byte {
        PING_RESPONSE = 0,
        GET_ACCOUNT_ACCESS = 1,
        GET_ACCOUNT_DATA = 2,
        LOGOUT = 3,
        REGISTER_ACOUNT = 4,
    }

    public class Request {
        private RequestType _type;
        private byte[] _data;
        private byte _requestId;



        public Request(byte[] data) {
            _requestId = data[0];
            _type = (RequestType) data[1];
            _data = data;
        }

        public RequestType Type => _type;

        public byte RequestId => _requestId;

        public byte[] Data => _data;

    }

    public class ResponsableRequest : Request {
        private Response _response;

        public Response Response => _response;

        public ResponsableRequest(Response response) : base(response.Data) {
            ServerManager.ServerMessageRecived += CatchResponse;
        }

        public ResponsableRequest(byte[] data) : base(data) {
            ServerManager.ServerMessageRecived += CatchResponse;
        }

        private void CatchResponse(Response response) {
            if (response.ResponseId != RequestId)
                return;
            ServerManager.ServerMessageRecived -= CatchResponse;
            _response = response;
            response.MarkHandled();
            OnResponded();
            ServerManager.RespondeRequest(this);
        }

        protected virtual void OnResponded() { }

    }
}