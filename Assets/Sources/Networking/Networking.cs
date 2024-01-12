using Networking.General;
using Networking.Lobbys;

namespace Networking
{
    public static class Networking
    {
        private static GeneralClient _generalClient;
        private static LobbyClient _lobbyClient;

        public static void Connect(string ip, int port)
        {
            _generalClient = new GeneralClient(ip, port);
        }

        public static void TryConnectToCombat()
        {

        }
    }
}
