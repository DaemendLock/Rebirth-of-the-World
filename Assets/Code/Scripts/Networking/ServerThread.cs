using System;

namespace Networking {
    public abstract class ServerThread {
        private const string SERVER = "localhost";

        public event Action<Response> ResponseRecived;



    }
}