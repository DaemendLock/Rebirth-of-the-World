using Networking.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Combat.Engine
{
    public static class CombatInitializer
    {
        public static void Initialize()
        {
            ConnectToServer();
            
            
        }

        private static void ConnectToServer()
        {
            Connector.TryConnectToCombat(0);
        }

        private static void SetupUnits()
        {

        }

        private static void AdjustPositions()
        {

        }

        private static void InsertData()
        {

        }
    }
}
