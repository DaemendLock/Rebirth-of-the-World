using Core.Combat.Abilities;
using Utils.DataStructure;
using Utils.DataTypes;
using View.Combat.Units;

namespace Adapters.Combat
{
    public static class ServerCommandsAdapter
    {
        public static void HandleCommand(byte[] data)
        {
            ByteReader reader = new ByteReader(data);

            switch ((ServerCommand) reader.ReadByte())
            {
                case ServerCommand.Cast:
                    HandleCastRequest(reader);
                    return;

                case ServerCommand.Move:
                    HandleMoveRequest(reader);
                    return;

                case ServerCommand.Target:
                    HandleTargetRequest(reader);
                    return;

                case ServerCommand.Stop:
                    HandleCandelRequest(reader);
                    return;

                case ServerCommand.CreateUnit:
                    HandleUnitCreationRequest(reader);
                    return;

                case ServerCommand.StartCombat:
                    HandleStartCombatRequest(reader);
                    return;
            }
        }

        private static void HandleStartCombatRequest(ByteReader source)
        {
            Core.Combat.Engine.Combat.Start();
        }

        private static void HandleMoveRequest(ByteReader source)
        {
            MoveData mdata = MoveData.Parse(source);

            Core.Combat.Engine.Units.MoveUnit(mdata.UnitId, mdata.Position, mdata.MoveDirection, mdata.Rotation);
        }

        private static void HandleCastRequest(ByteReader source)
        {
            CastData action = CastData.Parse(source);

            Core.Combat.Engine.Units.CastAbility(action.UnitId, action.TargetId, (SpellSlot) action.SpellSlot);
        }

        private static void HandleTargetRequest(ByteReader source)
        {
            TargetData action = TargetData.Parse(source);
            Core.Combat.Engine.Units.StartAttack(action.Attacker, action.Target);
        }

        private static void HandleCandelRequest(ByteReader source)
        {
            StopData action = StopData.Parse(source);
            Core.Combat.Engine.Units.StopAllActions(action.Unit);
        }

        private static void HandleUnitCreationRequest(ByteReader source)
        {
            UnitCreationData udata = UnitCreationData.Parse(source);

            Core.Combat.Engine.Units.CreateUnit(udata.Id, udata.Model);
            UnitFactory.CreateUnit(udata);
            SelectionInfo.RegisterControlUnit(udata.Id, udata.ControlGroup);
        }
    }
}
