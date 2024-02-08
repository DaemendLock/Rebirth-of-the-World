using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Lobby
{
    // Chat
    // Tasks
    // Connected players account data
    // Lfg

    public static class LobbyServer
    {
        private static Socket _server;
        private static Thread _thread;

        public static void Start()
        {
            if (_server != null)
            {
                return;
            }

            IPAddress address = IPAddress.Parse("localhost");
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _server.Bind(new IPEndPoint(IPAddress.Parse("localhost"), 3724));

            _thread = new Thread(DoServerThread);
            _thread.Start();
        }

        public static void Stop()
        {
            if (_server == null)
            {
                return;
            }

            _server.Close();
            _thread.Interrupt();
        }

        //OnPlayerConnect -> GeneralServer.RequestAccountData();

        private static void DoServerThread()
        {
            //while(_server.)
        }
    }
}