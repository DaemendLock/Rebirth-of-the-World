using System;
using Utils.DataTypes;
using Utils.Serializer.Input;

namespace Syncronization
{
    public static class CombatSyncroniaztion
    {
        public static event Action<MoveCommand> MoveRequested;
        public static event Action<CastCommand> CastRequested;
        public static event Action<TargetCommand> TargetRequested;
        public static event Action<StopCommand> CancelRequested;
        //public static event Action<UnitCreationData> UnitCreationRequested;

        public static void Put(byte[] data)
        {
            switch (data[0])
            {
                case (byte) InputCommand.Cast:
                    CastRequested?.Invoke(CastCommand.Parse(data, 1));
                    return;

                case (byte) InputCommand.Move:
                    MoveCommand moveCommand = MoveCommand.Parse(data, 1);
                    //TODO: check IsUnitControllable

                    MoveRequested?.Invoke(moveCommand);
                    return;

                case (byte) InputCommand.Target:
                    TargetRequested?.Invoke(TargetCommand.Parse(data, 1));
                    return;

                case (byte) InputCommand.Stop:
                    CancelRequested?.Invoke(StopCommand.Parse(data, 1));
                    return;
            }
        }

        public static void PutMoveCommand(MoveCommand data)
        {
            MoveRequested?.Invoke(data);
        }
    }
}
