namespace Networking
{
    public static class Combat
    {
        private static Utils.Client _client;

        public static void UseClient(Utils.Client client) => _client = client;

        public static bool TryConnectToCombat(int combatId) => false;

        public static void Send(byte[] data) => _client.SendRequest(data);
    }
}
