using System;
using System.IO;
using Utils.Serializer.Input;

namespace Utils.DataTypes
{
    public interface InputData
    {
        public byte[] GetBytes();
    }

    public readonly struct MoveCommand : InputData
    {
        private const int DATA_SIZE = 30;

        public readonly int UnitId;
        public readonly Vector3 Position;
        public readonly Vector3 MoveDirection;
        public readonly float Rotation;

        public MoveCommand(int id, Vector3 position, Vector3 moveDirection, float rotation)
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
                target.WriteByte((byte) InputCommand.Move);
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

        public static MoveCommand Parse(byte[] data, int start)
        {
            int id = BitConverter.ToInt32(data, start);
            Vector3 location = Vector3.Parse(data, start + sizeof(int));
            Vector3 moveDirection = Vector3.Parse(data, start + sizeof(int) + sizeof(float) * 3);
            float rotation = BitConverter.ToSingle(data, start + sizeof(int) + sizeof(float) * 6);

            return new(id, location, moveDirection, rotation);
        }
    }

    public readonly struct TargetCommand : InputData
    {
        private const int DATA_SIZE = 9;

        public readonly int Attacker;
        public readonly int Target;

        public TargetCommand(int attacker, int target)
        {
            Attacker = attacker;
            Target = target;
        }

        public static TargetCommand Parse(byte[] data, int start)
        {
            return new(BitConverter.ToInt32(data, start),
                BitConverter.ToInt32(data, start + sizeof(int)));
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(DATA_SIZE))
            {
                target.WriteByte((byte) InputCommand.Target);

                target.Write(BitConverter.GetBytes(Attacker));
                target.Write(BitConverter.GetBytes(Target));

                result = target.ToArray();
            }

            return result;
        }
    }

    public readonly struct CastCommand : InputData
    {
        private const int DATA_SIZE = 28;

        public readonly int UnitId;
        public readonly int TargetId;
        public readonly byte SpellSlot;

        public CastCommand(int unitId, int targetId, byte spellSlot)
        {
            UnitId = unitId;
            TargetId = targetId;
            SpellSlot = spellSlot;
        }

        public static CastCommand Parse(byte[] data, int start)
        {
            return new(BitConverter.ToInt32(data, start),
                BitConverter.ToInt32(data, start + sizeof(int)),
                data[start + sizeof(int) * 2]);
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(DATA_SIZE))
            {
                target.WriteByte((byte) InputCommand.Cast);
                target.Write(BitConverter.GetBytes(UnitId));
                target.Write(BitConverter.GetBytes(TargetId));
                target.WriteByte(SpellSlot);

                result = target.ToArray();
            }

            return result;
        }
    }

    public readonly struct StopCommand : InputData
    {
        private const int DATA_SIZE = 5;

        public readonly int Unit;

        public StopCommand(int unit)
        {
            Unit = unit;
        }

        public static StopCommand Parse(byte[] data, int start)
        {
            return new(BitConverter.ToInt32(data, start));
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(DATA_SIZE))
            {
                target.WriteByte((byte) InputCommand.Stop);

                target.Write(BitConverter.GetBytes(Unit));

                result = target.ToArray();
            }

            return result;
        }
    }
}
