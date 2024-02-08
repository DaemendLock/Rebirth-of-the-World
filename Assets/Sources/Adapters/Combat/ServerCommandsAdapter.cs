using Core.Combat.Abilities;
using Core.Combat.Engine;
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

                case ServerCommand.Kill:
                    return;

                case ServerCommand.Resurrect:
                    return;

                case ServerCommand.TakeDamage:
                    return;
            }
        }

        private static void HandleStartCombatRequest(ByteReader source)
        {
            Core.Combat.Engine.Combat.Start();
        }

        private static void HandleMoveRequest(ByteReader source)
        {
            MoveData moveData = MoveData.Parse(source);

            Units.MoveUnit(moveData.UnitId, moveData.Position, moveData.MoveDirection);
        }

        private static void HandleCastRequest(ByteReader source)
        {
            CastData action = CastData.Parse(source);
            //CastAction action = 
            Units.Cast(new (Units.GetUnitById(action.UnitId), Units.GetUnitById(action.TargetId), (SpellSlot)action.SpellSlot));

            //Clients.Send(_packetFactory.Create(action));

            //SchoolDamage.ApplyEffect(CastData data)->
            // {
            //      damageData = new DamageData()
            //      data.RecordNewDamage(damageData);
            //      Units.ApplyDamage(damageData);
            // }
            //Core.Combat.Engine----->
            //Units.DealDamage(damageData);
            //{
            //    DamageTaken?.Invoke(processedDamageData);
            //}

            //---------------

            //CastData data = CastData.Parse(source);
            //View.HandleCast(data);

            //View.Handle(action);
            //View.Combat.Units.Unit.GetUnit(action.Caster).CastAbility(action);
        }

        private static void HandleTargetRequest(ByteReader source)
        {
            TargetData action = TargetData.Parse(source);
            Units.StartAttack(action.Attacker, action.Target);
        }

        private static void HandleCandelRequest(ByteReader source)
        {
            StopData action = StopData.Parse(source);
            Units.StopAllActions(action.Unit);
        }

        private static void HandleUnitCreationRequest(ByteReader source)
        {
            UnitCreationData udata = UnitCreationData.Parse(source);

            Units.CreateUnit(udata.Id, udata.Model);
            UnitFactory.CreateUnit(udata);
            SelectionInfo.RegisterControlUnit(udata.Id, udata.ControlGroup);

        }
    }
}
