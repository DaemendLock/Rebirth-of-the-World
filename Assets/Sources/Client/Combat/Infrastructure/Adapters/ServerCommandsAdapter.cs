using Adapters.Combat.ActionHandlers;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using Utils.ByteHelper;
using Utils.DataTypes;

namespace Client.Combat.Infrastructure.Adapters
{
    public static class ServerCommandsAdapter
    {
        public static long Ping;

        public static void HandleUpdate(byte[] updateData)
        {
            HandleCommand(updateData);
        }

        public static void HandleCommand(byte[] data)
        {
            ByteReader reader = new ByteReader(data);

            long time = reader.ReadLong();

            Ping = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - time;

            int movementCount = reader.ReadInt();

            for (int i = 0; i < movementCount; i++)
            {
                int unit = reader.ReadInt();
                UnityEngine.Vector3 position = new UnityEngine.Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
                float rotation = reader.ReadFloat();
                UnityEngine.Vector3 velocity = new UnityEngine.Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());

                //TODO Unit.GetUnit(unit).UpdatePosition(position, velocity, rotation);
            }

            while (reader.HasNext)
            {
                try
                {
                    NextAction(reader);
                }
                catch (NotImplementedException @exception)
                {
                    Debug.Log("Missing implementaion: " + exception.Message);
                }
            }
        }

        private static ActionHandler NextAction(ByteReader source)
        {
            ActionType value = (ActionType) source.ReadByte();

            return value switch
            {
                ActionType.Dummy => new DummyActionHandler(source),
                ActionType.HealthModification => new HealthModificationActionHandler(source),
                ActionType.ApplyAura => throw new NotImplementedException(),
                ActionType.RemoveAura => throw new NotImplementedException(),
                ActionType.Kill => throw new NotImplementedException(),
                ActionType.Resurrect => throw new NotImplementedException(),
                ActionType.Precast => throw new NotImplementedException(),
                ActionType.StopCast => throw new NotImplementedException(),
                ActionType.ModifyResource => new ResourceModificationActionHandler(source),
                ActionType.Cast => new CastActionHandler(source),
                _ => throw new SerializationException($"Unknown action type {value}"),

            };
        }

        //private static void HandleStartCombatRequest(ByteReader source)
        //{
        //    Core.Combat.Engine.Combat.Start();
        //}

        //private static void HandleMoveRequest(ByteReader source)
        //{
        //    MoveData moveData = MoveData.Parse(source);

        //    Units.MoveUnit(moveData.UnitId, moveData.Position, moveData.MoveDirection);
        //}

        //private static void HandleCastRequest(ByteReader source)
        //{
        //    CastData action = CastData.Parse(source);

        //    Units.Cast(new (Units.GetUnitById(action.UnitId), Units.GetUnitById(action.TargetId), (SpellSlot)action.SpellSlot));

        //    //Clients.Send(_packetFactory.Create(action));

        //    //SchoolDamage.ApplyEffect(CastData data)->
        //    // {
        //    //      damageData = new DamageData()
        //    //      data.RecordNewDamage(damageData);
        //    //      Units.ApplyDamage(damageData);
        //    // }
        //    //Core.Combat.Engine----->
        //    //Units.DealDamage(damageData);
        //    //{
        //    //    DamageTaken?.Invoke(processedDamageData);
        //    //}

        //    //---------------

        //    //CastData data = CastData.Parse(source);
        //    //View.HandleCast(data);

        //    //View.Handle(action);
        //    //View.Combat.Units.Unit.GetUnit(action.Caster).CastAbility(action);
        //}

        //private static void HandleTargetRequest(ByteReader source)
        //{
        //    TargetData action = TargetData.Parse(source);
        //    Units.StartAttack(action.Attacker, action.Target);
        //}

        //private static void HandleCandelRequest(ByteReader source)
        //{
        //    StopData action = StopData.Parse(source);
        //    Units.StopAllActions(action.Unit);
        //}

        //private static void HandleUnitCreationRequest(ByteReader source)
        //{
        //    UnitCreationData udata = UnitCreationData.Parse(source);

        //    //Units.CreateUnit(udata.Id, udata.Model);
        //    UnitFactory.CreateUnit(udata);
        //    SelectionInfo.RegisterControlUnit(udata.Id, udata.ControlGroup);
        //}

    }
}