﻿using System;
using System.IO;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Adapters.Combat
{
    public readonly struct MoveData : InputData
    {
        private const int DATA_SIZE = 30;

        public readonly int UnitId;
        public readonly Vector3 Position;
        public readonly Vector3 MoveDirection;
        public readonly float Rotation;

        public MoveData(int id, Vector3 position, Vector3 moveDirection, float rotation)
        {
            UnitId = id;
            Position = position;
            MoveDirection = moveDirection;
            Rotation = rotation;
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(DATA_SIZE))
            {
                target.WriteByte((byte) ServerCommand.Move);
                target.Write(BitConverter.GetBytes(UnitId));

                target.Write(BitConverter.GetBytes(Position.x));
                target.Write(BitConverter.GetBytes(Position.y));
                target.Write(BitConverter.GetBytes(Position.z));

                target.Write(BitConverter.GetBytes(MoveDirection.x));
                target.Write(BitConverter.GetBytes(MoveDirection.y));
                target.Write(BitConverter.GetBytes(MoveDirection.z));

                target.Write(BitConverter.GetBytes(Rotation));

                result = target.ToArray();
            }

            return result;
        }

        public static MoveData Parse(ByteReader source)
        {
            int id = source.ReadInt();
            Vector3 location = Vector3.Parse(source);
            Vector3 moveDirection = Vector3.Parse(source);
            float rotation = source.ReadFloat();

            return new(id, location, moveDirection, rotation);
        }
    }

    public readonly struct TargetData : InputData
    {
        private const int DATA_SIZE = 9;

        public readonly int Attacker;
        public readonly int Target;

        public TargetData(int attacker, int target)
        {
            Attacker = attacker;
            Target = target;
        }

        public static TargetData Parse(ByteReader reader)
        {
            return new(reader.ReadInt(),
                reader.ReadInt());
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(DATA_SIZE))
            {
                target.WriteByte((byte) ServerCommand.Target);

                target.Write(BitConverter.GetBytes(Attacker));
                target.Write(BitConverter.GetBytes(Target));

                result = target.ToArray();
            }

            return result;
        }
    }

    public readonly struct CastData : InputData
    {
        private const int DATA_SIZE = 28;

        public readonly int UnitId;
        public readonly int TargetId;
        public readonly byte SpellSlot;

        public CastData(int unitId, int targetId, byte spellSlot)
        {
            UnitId = unitId;
            TargetId = targetId;
            SpellSlot = spellSlot;
        }

        public static CastData Parse(ByteReader source)
        {
            return new(source.ReadInt(),
                source.ReadInt(), source.ReadByte());
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(DATA_SIZE))
            {
                target.WriteByte((byte) ServerCommand.Cast);
                target.Write(BitConverter.GetBytes(UnitId));
                target.Write(BitConverter.GetBytes(TargetId));
                target.WriteByte(SpellSlot);

                result = target.ToArray();
            }

            return result;
        }
    }

    public readonly struct StopData : InputData
    {
        private const int DATA_SIZE = 5;

        public readonly int Unit;

        public StopData(int unit)
        {
            Unit = unit;
        }

        public static StopData Parse(ByteReader source)
        {
            return new (source.ReadInt());
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(DATA_SIZE))
            {
                target.WriteByte((byte) ServerCommand.Stop);

                target.Write(BitConverter.GetBytes(Unit));

                result = target.ToArray();
            }

            return result;
        }
    }
}
