using Core.Combat.Abilities;
using Utils.DataTypes;
using View.Combat.Units;

namespace Adapters.Combat
{
    public static class ServerCommandsAdapter
    {
        public static void HandleCommand(byte[] data)
        {
            switch (data[0])
            {
                case (byte) ServerCommand.Cast:
                    HandleCastRequest(data);
                    return;

                case (byte) ServerCommand.Move:
                    HandleMoveRequest(data);
                    return;

                case (byte) ServerCommand.Target:
                    HandleTargetRequest(data);
                    return;

                case (byte) ServerCommand.Stop:
                    HandleCandelRequest(data);
                    return;

                case (byte) ServerCommand.CreateUnit:
                    HandleUnitCreationRequest(data);
                    return;
            }
        }

        private static void HandleMoveRequest(byte[] data)
        {
            MoveData mdata = MoveData.Parse(data, 1);

            Core.Combat.Engine.Combat.MoveUnit(mdata.UnitId, mdata.Position, mdata.MoveDirection, mdata.Rotation);
        }

        private static void HandleCastRequest(byte[] data)
        {
            CastData action = CastData.Parse(data, 1);

            Core.Combat.Engine.Combat.CastAbility(action.UnitId, action.TargetId, (SpellSlot) action.SpellSlot);
        }

        private static void HandleTargetRequest(byte[] data)
        {
            TargetData action = TargetData.Parse(data, 1);
            Core.Combat.Engine.Combat.StartAttack(action.Attacker, action.Target);
        }

        private static void HandleCandelRequest(byte[] data)
        {
            StopData action = StopData.Parse(data, 1);
            Core.Combat.Engine.Combat.StopAllActions(action.Unit);
        }

        private static void HandleUnitCreationRequest(byte[] data)
        {
            UnitCreationData udata = UnitCreationData.Parse(data, 1);

            Core.Combat.Engine.Combat.CreateUnit(udata);
            UnitFactory.CreateUnit(udata);
            SelectionInfo.RegisterControllUnit(udata.Id, udata.ControlGroup);
        }
    }
}
