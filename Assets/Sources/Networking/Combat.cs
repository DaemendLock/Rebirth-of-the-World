using Adapters;
using Networking.Utils;

namespace Networking
{
    public static class Combat
    {
        private static Client _client;

        public static void UseClient(Client client)
        {
            _client = client;
        }

        public static bool TryConnectToCombat(int combatId)
        {
            return false;
        }

        public static void Send(InputData data)
        {
            _client.SendRequest(data.GetBytes());
        }
    }
}
