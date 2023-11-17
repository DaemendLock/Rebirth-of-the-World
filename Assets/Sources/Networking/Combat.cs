using Networking.Utils;
using Syncronization;
using Utils.DataTypes;

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

        public static void Send(MoveCommand data)
        {
            CombatSyncroniaztion.PutMoveCommand(data);
            _client.SendRequest(data.GetBytes());
        }

        public static void Send(CastCommand data)
        {
            _client.SendRequest(data.GetBytes());
        }

        public static void Send(TargetCommand data)
        {
            _client.SendRequest(data.GetBytes());
        }

        public static void Send(StopCommand data)
        {
            _client.SendRequest(data.GetBytes());
        }
    }
}
