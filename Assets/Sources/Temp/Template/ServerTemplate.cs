using Core.Combat.Engine;
using Core.Combat.Utils;
using Input;
using View.Combat.Units;

namespace Assets.Sources.Temp.Template
{
    enum ServerCommand : byte
    {
        TARGET,
        STOP,
        CREATE_UNIT
    }

    internal static class ServerTemplate
    {
        public static void HandleData(byte[] data)
        {
            ServerCommand command = (ServerCommand) data[0];

            switch (command)
            {
                case ServerCommand.CREATE_UNIT:
                    HandleCreateUnitCommand(data);
                    return;

                default:
                    return;
            }
        }

        #region Input
        private static void HandleCreateUnitCommand(byte[] data)
        {
            SendUpdate(data);
        }
        #endregion

        #region Output
        private static void SendUpdate(byte[] data)
        {
            ServerCommandSupplier.Put(data);
        }

        #endregion
    }

    public static class ServerCommandSupplier
    {
        public static void Put(byte[] data)
        {
            HandleServerCommand(data);
        }

        private static void HandleServerCommand(byte[] data)
        {
            switch (data[0])
            {
                case (byte) ServerCommand.CREATE_UNIT:
                    CreateUnit(data);
                    break;
            }
        }

        private static void CreateUnit(byte[] data)
        {
            int start = 1;

            UnitCreationData udata = UnitCreationData.Parse(data, ref start);

            Combat.CreateUnit(udata);
            UnitFactory.CreateUnit(udata);
            SellectionInfo.RegisterControllUnit(udata.Id, data[start]);
        }
    }
}
