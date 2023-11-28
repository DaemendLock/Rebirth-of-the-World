namespace Core.Combat.Engine
{
    public static class CombatInitializer
    {
        public static void Initialize()
        {
            ConnectToServer();

            Combat.Start();
        }

        private static void ConnectToServer()
        {
            //Connector.TryConnectToCombat(0);
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
