using System.Net.Sockets;

namespace Remaster.Net.Networking
{
    public static class Connector
    {
        public static TcpClient GetCombatConnection(string ip, int port)
        {
            TcpClient result = new TcpClient(ip, port);

            return result;
        }
    }
}
