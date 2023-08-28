using Remaster.Net.Combat;
using Remaster.Networking.Lobby;
using System;

namespace Remaster.Networking
{
    public static class Server
    {
        private static CombatClient _combatClient;
        private static LobbyClient _lobbyClient;

        public static void TryConnectToCombat(int combatId)
        {

        }

        public static void SendCastRequest(Unit caster, Unit target, SpellSlot slot)
        {
            throw new NotImplementedException();
        }

        public static void SendMovement()
        {

        }
    }
}
