using System.Net;
using System.Net.Sockets;

namespace Server.General
{

    //What to do:
    // AccountData
    // 

    public static class GeneralServer
    {
        private static TcpListener _server;

        public static void Start()
        {
            if (_server != null)
            {
                return;
            }

            IPAddress address = IPAddress.Parse("localhost");
            _server = new(address, 27122);

            _server.Start();
        }

        public static void Stop()
        {
            if (_server == null)
            {
                return;
            }
        }
    }
}